"use client";

import { createContext, useContext, useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";
import { UserClaims } from "@/types/auth";

type AuthContextType = {
    user: UserClaims | null;
    isAuthenticated: boolean;
    loading: boolean;
    login: (token: string) => void;
    logout: () => void;
    getToken: () => string;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

const TOKEN_KEY = "user_token";

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [user, setUser] = useState<UserClaims | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem(TOKEN_KEY);

        if (!token) {
            setLoading(false);
            return;
        }
        setLoading(true);


        try {
            const decoded = jwtDecode<UserClaims>(token);
            const currentTime = Date.now() / 1000;

            if (decoded.exp < currentTime) {
                localStorage.removeItem(TOKEN_KEY);
            } else {
                setUser(decoded);
            }
        } catch {
            localStorage.removeItem(TOKEN_KEY);
        }

        setLoading(false);
    }, []);

    const login = (token: string) => {
        localStorage.setItem(TOKEN_KEY, token);

        const decoded = jwtDecode<UserClaims>(token);
        setUser(decoded); // 🔥 força re-render
    };

    const logout = () => {
        localStorage.removeItem(TOKEN_KEY);
        setUser(null);
    };

    const getToken = (): string => {
        var token = localStorage.getItem(TOKEN_KEY);
        return token ? token : "";
    };

    return (
        <AuthContext.Provider
            value={{
                user,
                isAuthenticated: !!user,
                loading,
                login,
                logout,
                getToken
            }}
        >
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const context = useContext(AuthContext);

    if (!context) {
        throw new Error("useAuth must be used inside AuthProvider");
    }

    return context;
}
