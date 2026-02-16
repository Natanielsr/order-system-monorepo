"use client"; // Este componente será interativo

import { useState } from 'react';
import { Product } from '@/types/product';
import { ShoppingCart, Search } from 'lucide-react';
import { useCart } from '@/context/CartContext';
import ProductImage from './ProductImage';
import SimpleAlert from './SimpleAlert';

interface ProductListProps {
    initialProducts: Product[]; // Recebe os dados vindos do servidor
}

export default function ProductList({ initialProducts }: ProductListProps) {
    const [alert, setAlert] = useState({ show: false, message: '', type: 'success' as 'success' | 'error' });

    const [searchTerm, setSearchTerm] = useState("");
    const { addToCart, openCart } = useCart();


    // Lógica de filtro no cliente
    const filteredProducts = initialProducts.filter(p =>
        p.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="space-y-6">
            {/* Barra de Busca - Interatividade que exige 'use client' */}
            <div className="relative max-w-md">
                <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 w-5 h-5" />
                <input
                    type="text"
                    placeholder="Buscar produtos..."
                    className="w-full pl-10 pr-4 py-2 border rounded-xl outline-none focus:ring-2 focus:ring-orange-500"
                    onChange={(e) => setSearchTerm(e.target.value)}
                />
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
                {filteredProducts.map((product: Product) => (
                    <div key={product.id} className="border border-gray-200 shadow-md rounded-lg p-4 bg-white hover:shadow-md transition-shadow">
                        <div className='flex justify-center items-center h-64'>
                            <ProductImage product={product} width={200} height={200}></ProductImage>
                        </div>
                        <h3 className="font-bold text-gray-800">{product.name}</h3>
                        <p className="text-blue-600 font-bold">R$ {product.price.toFixed(2)}</p>
                        <div><label className='font-bold'>Disponível:</label><span className='ml-2'>{product.availableQuantity}</span></div>
                        <button
                            onClick={() => {
                                setAlert({ show: true, message: `${product.name} Adicionado ao Carrinho!`, type: 'success' });
                                addToCart(product);
                                openCart(); // 👈 abre o sidebar automaticamente
                            }}
                            className="w-full mt-4 bg-orange-500 text-white py-2 rounded-lg hover:bg-orange-600"
                        >
                            Adicionar
                        </button>
                    </div>
                ))}
            </div>
            <SimpleAlert
                isOpen={alert.show}
                message={alert.message}
                type={alert.type}
                onClose={() => setAlert({ ...alert, show: false })}
            />
        </div>
    );
}