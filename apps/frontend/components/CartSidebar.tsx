import { formatCurrency } from "@/utils/format";
import { useCart } from "@/context/CartContext";
import Link from "next/link";
import Button from "./Button";
import ProductImage from "./ProductImage";

export default function CartSidebar() {
    const { cart, removeFromCart, total, isOpen, closeCart } = useCart();

    return (
        <div className="fixed top-12 right-0 w-32 h-[calc(100vh-48px)] shadow-lg border-l border-gray-200 bg-white flex flex-col">
            <div className="flex-none shadow-sm">
                <div className="text-center">
                    <div>
                        <span>Subtotal</span>
                    </div>
                    <span className="justify-center font-bold text-red-700">
                        {formatCurrency(total)}
                    </span>
                </div>
                <div className="m-1 ml-2">

                    <Link href="/cart">
                        <Button className="text-xs">Ir Para o Carrinho</Button>
                    </Link>
                </div>
            </div>
            <div className="flex-1 overflow-y-auto overflow-x-hidden custom-scrollbar">
                {cart.length === 0 ? <div className="py-5 px-2">Seu Carrinho está vázio</div> : (
                    <>
                        {cart.map((item) => (
                            <div key={item.id}>
                                <div className="ml-2 mr-2">
                                    <div className="mt-2 flex justify-center items-center">
                                        <ProductImage product={item} width={100} height={100}></ProductImage>
                                    </div>

                                    <strong>{item.name}</strong>
                                    <div style={{ fontSize: "14px", color: "#666" }}>
                                        {item.quantity}x {formatCurrency(item.price)}
                                    </div>
                                    <button onClick={() => removeFromCart(item.id)} style={{ color: "red", fontSize: "12px", cursor: "pointer" }}>
                                        Remover
                                    </button>
                                </div>
                                <hr />
                            </div>

                        ))}


                    </>
                )}
            </div>
        </div>
    );
}