'use client'

import CartSidebar from "./CartSidebar"
import Header from "./Header"
import React from "react";
import { useCart } from "@/context/CartContext";

type AppProps = {
    children: React.ReactNode;
};

export default function App({ children }: AppProps) {
    const { isOpen } = useCart();

    return (
        <div>
            <div className={`${isOpen ? "md:mr-32" : "md:mr-0"} pt-12`}>
                <Header />
                <main className="p-4 bg-gray-50">
                    {children}
                </main>
            </div>
            {isOpen && (
                <div className="hidden md:block">
                    <CartSidebar />
                </div>
            )}

        </div>
    )
}