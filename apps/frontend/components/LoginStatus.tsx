"use client";

import { useAuth } from "@/context/AuthContext";
import { LogInIcon } from "lucide-react";
import Link from "next/link";
import UserDropdown from "./UserDropdown";

export default function LoginStatus() {
    const { user, isAuthenticated, loading } = useAuth();

    return <div className="flex">
        {isAuthenticated ? (
            <div className="flex items-center justify-center mr-2">
                <UserDropdown username={user?.unique_name ?? ""} />
            </div>
        ) : (
            <Link href="/login">
                <button
                    className='flex items-center gap-2 bg-blue-600 hover:bg-blue-700 px-4 py-2 rounded-lg transition-colors mr-2'>
                    <LogInIcon size={16} />
                    Login
                </button>
            </Link>
        )}
    </div>
}