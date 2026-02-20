import { Product } from "./product";

export interface CartItem extends Product {
    quantity: number;
}

export interface CartContextData {
    cart: CartItem[];
    addToCart: (product: Product) => void;
    removeFromCart: (id: string) => void;
    total: number;
    totalItens: number;
    increaseItem: (id: string) => void;
    decreaseItem: (id: string) => void;
    isOpen: boolean;
    openCart: () => void;
    closeCart: () => void;
    clearCart: () => void;
    loadingCart: boolean;
}


