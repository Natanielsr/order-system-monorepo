// context/CartContext.tsx
"use client";
import { createContext, useContext, useState, ReactNode, useEffect } from "react";
import { CartItem, CartContextData } from "../types/cart";
import { Product } from "@/types/product";

const CartContext = createContext<CartContextData | undefined>(undefined);

export const CartProvider = ({ children }: { children: ReactNode }) => {
    const [cart, setCart] = useState<CartItem[]>([]);
    const [totalItens, setTotalItens] = useState<number>(0);
    const [isOpen, setIsOpen] = useState(false);
    const [loadingCart, setLoadingCart] = useState(true);

    const openCart = () => setIsOpen(true);
    const closeCart = () => setIsOpen(false);

    // 1. CARREGAR os dados ao iniciar
    useEffect(() => {
        const savedCart = localStorage.getItem("@MeuApp:carrinho");
        if (savedCart) {
            setCart(JSON.parse(savedCart));
        }

    }, []);

    // 2. SALVAR os dados sempre que o carrinho for alterado
    useEffect(() => {
        // Evita salvar um carrinho vazio por cima dos dados reais no primeiro render
        if (cart.length > 0) {
            localStorage.setItem("@MeuApp:carrinho", JSON.stringify(cart));

        } else if (cart.length === 0) {
            // Se o carrinho foi limpo propositalmente, remove do storage
            localStorage.removeItem("@MeuApp:carrinho");
        }

        const total = cart.reduce((acc, item) => acc + item.quantity, 0);
        setTotalItens(total);

        setLoadingCart(false);
    }, [cart]);

    const addToCart = (product: Product) => {
        setCart((prev) => {
            const existing = prev.find((item) => item.id === product.id);
            if (existing) {
                return prev.map((item) =>
                    item.id === product.id ? { ...item, quantity: item.quantity + 1 } : item
                );
            }

            return [...prev, { ...product, quantity: 1 }];
        });
        setTotalItens(totalItens + 1);
        setLoadingCart(false);
    };

    const removeFromCart = (id: string) => {
        var item = cart.find(i => i.id == id);
        if (item != null)
            setTotalItens(totalItens - item?.quantity);

        setCart((prev) => prev.filter((item) => item.id !== id));

        setLoadingCart(false);
    };

    const increaseItem = (id: string) => {
        setCart(prevCart =>
            prevCart.map(item =>
                item.id === id
                    ? { ...item, quantity: item.quantity + 1 }
                    : item
            )
        );

    };

    const decreaseItem = (id: string) => {
        setCart(prevCart =>
            prevCart.map(item =>
                item.id === id
                    ? { ...item, quantity: item.quantity - 1 }
                    : item
            )
                .filter(item => item.quantity > 0)
        );

    };

    const clearCart = () => {
        setCart([]);
    };


    const total = cart.reduce((acc, item) => acc + item.price * item.quantity, 0);

    return (
        <CartContext.Provider value={{
            cart,
            addToCart,
            removeFromCart,
            total,
            totalItens,
            increaseItem,
            decreaseItem,
            isOpen,
            openCart,
            closeCart,
            clearCart,
            loadingCart
        }}>
            {children}
        </CartContext.Provider>
    );
};

export const useCart = () => {
    const context = useContext(CartContext);
    if (!context) throw new Error("useCart deve ser usado dentro de um CartProvider");
    return context;
};