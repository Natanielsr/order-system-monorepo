"use client";

import Button from "@/components/Button";
import CartItemComponent from "@/components/CartItemComponent";
import { useCart } from "@/context/CartContext";
import { formatCurrency } from "@/utils/format";
import { ChevronRight } from "lucide-react";
import Link from "next/link";
import { useEffect } from "react";

export default function Cart() {
    const { cart, removeFromCart, total, increaseItem, decreaseItem, closeCart, totalItens } = useCart();

    useEffect(() => {
        closeCart();
    });

    return <div>
        <h1 className="font-bold mb-5">Carrinho</h1>
        <main className="grid grid-cols-1 lg:grid-cols-3 gap-8 items-start">
            <div className="lg:col-span-2 space-y-4">
                {cart.map(i =>
                    <div key={i.id} className="mb-2">
                        <CartItemComponent
                            cartItem={i}
                            onRemove={() => removeFromCart(i.id)}
                            onIncrease={() => increaseItem(i.id)}
                            onDecrease={() => decreaseItem(i.id)}
                        ></CartItemComponent>
                    </div>

                )}

            </div>

            <div className="lg:col-span-1">
                <div className="border border-gray-200 rounded-lg p-5 sticky top-4">

                    {totalItens > 0 ? (
                        <Link href="/checkout">
                            <button
                                className="w-full rounded-lg py-2 text-sm shadow-sm font-medium bg-[#FFD814] hover:bg-[#F7CA00] border-[#FCD200] text-gray-900"
                            >
                                Fechar Pedido
                            </button>
                        </Link>
                    ) : (
                        <button
                            disabled
                            className="w-full rounded-lg py-2 text-sm shadow-sm font-medium bg-gray-300 text-gray-500 cursor-not-allowed"
                        >
                            Fechar Pedido
                        </button>
                    )}

                    <hr className="my-4 border-gray-200" />

                    <h3 className="font-bold text-lg mb-3">Resumo do pedido</h3>
                    <div className="space-y-2 text-sm text-gray-700">
                        <div className="flex justify-between">
                            <span>Itens:</span>
                            <span>{formatCurrency(total)}</span>
                        </div>
                    </div>

                    <hr className="my-3 border-gray-200" />

                    <div className="flex justify-between text-lg font-bold text-[#B12704]">
                        <span>Total do pedido:</span>
                        <span>{formatCurrency(total)}</span>
                    </div>
                </div>
            </div>
        </main>
    </div >
}