"use client";

import { useState } from "react";
import Link from "next/link";
import { ChevronDown } from "lucide-react";
import { useAuth } from "@/context/AuthContext";

type Props = {
    username: string;
};

export default function UserDropdown({ username }: Props) {
    const [open, setOpen] = useState(false);
    const { logout } = useAuth();

    return (
        <div
            className="relative inline-block text-left"
            onMouseEnter={() => setOpen(true)}
            onMouseLeave={() => setOpen(false)}
        >
            {/* Botão */}
            <button className="flex items-center gap-1 px-3 py-2 bg-blue-900 rounded-lg hover:bg-blue-600 transition-colors">
                <span className="text-sm font-medium">
                    Olá, {username}
                </span>
                <ChevronDown size={16} />
            </button>

            {/* Dropdown */}
            <div
                className={`absolute right-0 top-full w-48 bg-white rounded-xl shadow-lg border transition-all duration-200 origin-top ${open
                    ? "opacity-100 scale-100"
                    : "opacity-0 scale-95 pointer-events-none"
                    }`}
            >
                <div className="py-2 text-sm text-gray-700">
                    <Link
                        href="/profile"
                        className="block px-4 py-2 hover:bg-gray-100"
                    >
                        Meu Perfil
                    </Link>

                    <Link
                        href="/order"
                        className="block px-4 py-2 hover:bg-gray-100"
                    >
                        Meus Pedidos
                    </Link>

                    <hr className="my-2" />

                    <button
                        className="w-full text-left px-4 py-2 hover:bg-red-100 text-red-600"
                        onClick={logout}
                    >
                        Sair
                    </button>
                </div>
            </div>
        </div>
    );
}
