"use client";

import { CartItem } from "@/types/cart";
import { formatCurrency } from "@/utils/format";
import ProductImage from "./ProductImage";

interface CartItemProps {
    cartItem: CartItem,
    onIncrease?: () => void;
    onDecrease?: () => void;
    onRemove?: () => void;
}

export default function CartItemComponent({
    cartItem,
    onIncrease,
    onDecrease,
    onRemove
}: CartItemProps) {
    return (
        <div
            className="
        flex gap-4
        bg-white
        border border-gray-200
        rounded-lg
        p-4
        shadow-md
      "
        >
            {/* Imagem */}
            <div className="flex items-center justify-center">
                <ProductImage product={cartItem} width={100} height={100} ></ProductImage>
            </div>


            {/* Conteúdo */}
            <div className="flex flex-col justify-between flex-1">
                <div>
                    <h3 className="font-medium text-base">{cartItem.name}</h3>
                    <p className="text-sm text-gray-600">
                        {formatCurrency(cartItem.price)}
                    </p>
                </div>

                {/* Ações */}
                <div className="flex items-center justify-between mt-2">
                    <div className="flex items-center gap-2">
                        <button
                            onClick={onDecrease}
                            className="cursor-pointer w-8 h-8 border border-black rounded text-lg"
                        >
                            −
                        </button>

                        <span className="text-center">
                            {cartItem.quantity}
                        </span>

                        <button
                            onClick={onIncrease}
                            className="cursor-pointer w-8 h-8 border border-black rounded text-lg"
                        >
                            +
                        </button>
                    </div>

                    <button
                        onClick={onRemove}
                        className="cursor-pointer text-sm text-red-600 hover:underline"
                    >
                        Remover
                    </button>
                </div>
            </div>
        </div>
    );
}
