// components/ProductCard.tsx
"use client";

import Image from 'next/image';
import CartIcon from './CartIcon';
import { formatCurrency } from '@/utils/format';
import { Product } from '@/types/product';

interface ProductCardProps {
    product: Product;
    onClick: any;
}

export default function ProductCard({ product, onClick }: ProductCardProps) {
    return (
        <div className="max-w-sm rounded-2xl overflow-hidden shadow bg-gray-300 p-2 border border-gray-200">
            <div className="relative aspect-square w-full mb-1 bg-white rounded-md overflow-hidden shadow">
                <Image
                    src={product.imagePath}
                    alt={product.name}
                    fill
                    className="object-contain rounded-md shadow"
                />
            </div>
            <h3 className="text-center font-bold text-xl mb-1 text-gray-800">{product.name}</h3>
            <p className="text-center text-blue-600 font-semibold text-lg">{formatCurrency(product.price)}</p>
            <button
                className="cursor-pointer mt-2 w-full bg-black hover:bg-gray-800 text-white font-medium py-2.5 px-2 rounded-lg flex items-center justify-center gap-2 transition-all active:scale-95 shadow"
                onClick={onClick}
            >
                <CartIcon></CartIcon>
                <span>Adicionar ao carrinho</span>
            </button>
            <button className="cursor-pointer mt-4 w-full bg-black text-white py-2 rounded-lg hover:bg-gray-800 transition shadow">
                Comprar
            </button>
        </div>
    );
}