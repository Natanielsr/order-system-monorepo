"use client";

import { useState } from "react";
import { Menu, X } from "lucide-react";
import { useAuth } from "@/context/AuthContext";
import { useRouter } from "next/navigation";

export default function MobileSidebar() {
    const [open, setOpen] = useState(false);
    const { logout, isAuthenticated } = useAuth();
    const router = useRouter();

    const handleLogout = () => {
        logout();
        setOpen(false);
        router.push("/login");
    };

    return (
        <>
            {/* Botão hamburger */}
            <button
                onClick={() => setOpen(true)}
                className="p-2 md:hidden"
            >
                <Menu size={28} />
            </button>

            {/* Overlay */}
            {open && (
                <div
                    className="fixed inset-0 bg-black/50 z-40"
                    onClick={() => setOpen(false)}
                />
            )}

            {/* Sidebar */}
            <div
                className={`fixed top-0 left-0 h-full w-64 bg-white text-black shadow-lg z-50 transform transition-transform duration-300
        ${open ? "translate-x-0" : "-translate-x-full"}`}
            >
                {/* Header */}
                <div className="flex justify-between items-center p-4 border-b">
                    <span className="font-bold text-lg">Menu</span>
                    <button onClick={() => setOpen(false)}>
                        <X size={24} />
                    </button>
                </div>

                {/* Itens */}
                {isAuthenticated ?
                    <nav className="flex flex-col p-4 gap-4">
                        <a href="/" className="hover:text-blue-600">Home</a>
                        <a href="/cart" className="hover:text-blue-600">Carrinho</a>
                        <a href="/order" className="hover:text-blue-600">Pedidos</a>
                        <a href="/profile" className="hover:text-blue-600">Perfil</a>
                        <a onClick={handleLogout} className="cursor-pointer hover:text-blue-600">Sair</a>
                    </nav>
                    :
                    <nav className="flex flex-col p-4 gap-4">
                        <a href="/" className="hover:text-blue-600">Home</a>
                        <a href="/login" className="hover:text-blue-600">Login</a>
                        <a href="/cart" className="hover:text-blue-600">Carrinho</a>
                    </nav>
                }
            </div>
        </>
    );
}
