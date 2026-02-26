"use client";

import { Lock, ChevronRight } from 'lucide-react';
import { useCart } from '@/context/CartContext';
import { formatCurrency } from '@/utils/format';
import Link from 'next/link';
import { useEffect, useState } from 'react';
import ProductImage from '@/components/ProductImage';
import SimpleAlert from '@/components/SimpleAlert';
import { useRouter, usePathname } from 'next/navigation';
import { useAuth } from '@/context/AuthContext';
import PaymentSelector from '@/components/PaymentSelector';
import Spinner from '@/components/Spinner';
import AddressSelector from '@/components/AddressSelector';
import { Address } from '@/types/address';

export default function CheckoutPage() {
    const { cart, total, totalItens, closeCart, clearCart, loadingCart } = useCart();
    const [isPending, setIsPending] = useState(false);
    const { user, isAuthenticated, loading, getToken } = useAuth();
    const [alert, setAlert] = useState({ show: false, message: '', type: 'success' as 'success' | 'error' });
    const router = useRouter();
    const pathname = usePathname(); // Hook para pegar a URL atual
    const [selectedPayment, setSelectedPayment] = useState(0);
    const [address, setAddress] = useState<Address>();

    const showErrorAlert = (message: string) => {
        setAlert({ show: true, message: message, type: 'error' });
    };

    useEffect(() => {
        if (loading) return;

        if (!isAuthenticated) {
            router.push("/login");
        }

        if (!loadingCart) {
            if (totalItens === 0)
                router.push("/");
        }

        closeCart();

    }, [loading, isAuthenticated, loadingCart, totalItens, router]);

    async function handleConfirmOrder() {
        setIsPending(true);

        var token = getToken();

        const orderData = {
            userId: user?.nameid,
            orderItems: cart.map(item => ({
                productId: item.id,
                quantity: item.quantity
            })),
            paymentMethod: selectedPayment,
            addressId: address?.id
        };

        try {
            const response = await fetch('http://localhost:5012/api/order', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}` // Se necessário
                },
                body: JSON.stringify(orderData),
            });

            if (!response.ok) {
                const errorData = await response.json().catch(() => ({}));
                showErrorAlert("Houve um erro ao confirmar seu pedido. Tente novamente.");
                console.log(errorData);

                // ✅ Retorne um objeto, não lance erro
                return;
            }

            const result = await response.json();

            // Sucesso: Redirecionar para página de confirmação
            clearCart();
            window.location.href = '/checkout/success?id=' + result.orderId;

        } catch (error) {
            console.error('Erro inesperado:', error);

            return {
                success: false,
                error: 'Erro de conexão. Tente novamente.',
                isNetworkError: true
            };

        } finally {
            setIsPending(false);
        }
    }

    const handleChangeAddress = (selAddress: Address) => {
        setAddress(selAddress);
        console.log(address);
    };

    if (loading || (!isAuthenticated && pathname !== '/login')) {
        return <Spinner />; // Ou apenas return null;
    }




    return (
        <div className="min-h-screen bg-white">
            {user?.role === 'Admin' && (
                <div className='bg-amber-300 p-2'>
                    {/* O TS sabe que 'user' tem 'username' e 'role' */}
                    <h1>Olá, {user?.unique_name}!</h1>
                    <p>Seu ID de usuário é: {user?.nameid}</p>


                    <span className="bg-red-100 text-red-800 p-1 rounded text-xs">
                        Painel de Admin
                    </span>

                </div>
            )}

            {/* Header Simplificado (Foco total na conversão) */}
            <header className="border-b border-gray-200 bg-gray-50 py-3">
                <div className="max-w-6xl mx-auto px-4 flex justify-between items-center">
                    <h1 className="text-2xl font-bold text-gray-800">Confirmar Pedido</h1>
                    <Lock className="text-gray-400 w-5 h-5" />
                </div>
            </header>
            <form onSubmit={(e) => {
                e.preventDefault();
                handleConfirmOrder();
            }}>
                <main className="max-w-6xl mx-auto p-4 md:p-8 grid grid-cols-1 lg:grid-cols-3 gap-8">

                    {/* Coluna da Esquerda: Fluxo de compra */}
                    <div className="lg:col-span-2 space-y-6">

                        {/* Passo 1: Endereço */}
                        <div className="border-b border-gray-200 pb-6">
                            <div className="flex gap-4">
                                <span className="text-lg font-bold text-gray-700">1</span>
                                <div className="flex-1">
                                    <div className="flex justify-between items-start">
                                        <h3 className="text-lg font-bold">Endereço de envio</h3>
                                        <Link href="/address">
                                            <button className="cursor-pointer text-blue-600 hover:text-orange-600 text-sm">Alterar</button>
                                        </Link>
                                    </div>
                                    <AddressSelector
                                        onSelect={handleChangeAddress}
                                    />
                                    <p className="text-sm text-gray-600 mt-1">
                                        {address?.fullName}<br />
                                        {address?.street}, {address?.number}<br />
                                        {address?.city}, {address?.state} {address?.zipCode}
                                    </p>
                                </div>
                            </div>
                        </div>

                        {/* Passo 2: Pagamento */}
                        <div className="border-b border-gray-200 pb-6">
                            <div className="flex gap-4">
                                <span className="text-lg font-bold text-gray-700">2</span>
                                <div className="flex-1">
                                    <div className="flex justify-between items-start">
                                        <h3 className="text-lg font-bold">Método de pagamento</h3>
                                    </div>
                                    <div className="mt-2 flex items-center gap-2">
                                        <PaymentSelector value={selectedPayment} onChange={setSelectedPayment}></PaymentSelector>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {/* Passo 3: Revisão de Itens */}
                        <div className="pb-6">
                            <div className="flex gap-4">
                                <span className="text-lg font-bold text-gray-700">3</span>
                                <div className="flex-1">
                                    <h3 className="text-lg font-bold mb-4">Revisar itens e envio</h3>

                                    {cart.map(i =>
                                        <div key={i.id} className="mb-2 border border-gray-200 shadow-md rounded-lg p-4 space-y-4">
                                            <div className="flex gap-4">
                                                <div className="w-20 h-20 bg-gray-100 rounded flex items-center justify-center">
                                                    <ProductImage product={i} height={80} width={80} ></ProductImage>
                                                </div>
                                                <div className="flex-1">
                                                    <h4 className="font-bold text-sm text-gray-900">{i.name}</h4>
                                                    <p className="text-xs text-green-600 font-bold mt-1">Em estoque</p>
                                                    <p className="text-xs text-gray-500 mt-1">Quantidade: {i.quantity}</p>
                                                </div>
                                            </div>
                                        </div>
                                    )}

                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Coluna da Direita: Resumo Final (Sidebar) */}
                    <div className="lg:col-span-1">
                        <div className="border border-gray-200 rounded-lg p-5 sticky top-4">

                            <button
                                type="submit"
                                disabled={isPending}
                                className={`cursor-pointer w-full rounded-lg py-2 text-sm shadow-sm font-medium transition-colors
                                ${isPending
                                        ? 'bg-gray-200 cursor-not-allowed text-gray-500 border-gray-300'
                                        : 'bg-[#FFD814] hover:bg-[#F7CA00] border-[#FCD200] text-gray-900'
                                    }
                            `}>
                                {isPending ? (
                                    <span className="flex items-center justify-center gap-2">
                                        {/* Spinner simples */}
                                        <svg className="animate-spin h-4 w-4 text-gray-600" viewBox="0 0 24 24">
                                            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                                            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
                                        </svg>
                                        Processando...
                                    </span>
                                ) : (
                                    "Confirmar pedido"
                                )}

                            </button>


                            <p className="text-[11px] text-gray-500 text-center mt-3 leading-tight">
                                Ao confirmar o pedido, você concorda com as Condições de Uso da nossa loja.
                            </p>

                            <hr className="my-4 border-gray-200" />

                            <h3 className="font-bold text-lg mb-3">Resumo do pedido</h3>
                            <div className="space-y-2 text-sm text-gray-700">
                                <div className="flex justify-between">
                                    <span>Itens:</span>
                                    <span>{formatCurrency(total)}</span>
                                </div>
                                <div className="flex justify-between">
                                    <span>Frete e manuseio:</span>
                                    <span>R$ 0,00</span>
                                </div>
                            </div>

                            <hr className="my-3 border-gray-200" />

                            <div className="flex justify-between text-lg font-bold text-[#B12704]">
                                <span>Total do pedido:</span>
                                <span>{formatCurrency(total)}</span>
                            </div>

                            <div className="mt-4 bg-gray-100 p-3 rounded-b-lg -mx-5 -mb-5 border-t border-gray-200">
                                <button type='button' className="text-blue-600 hover:text-orange-600 hover:underline text-xs flex items-center">
                                    Como os custos de frete são calculados? <ChevronRight className="w-3 h-3" />
                                </button>
                            </div>
                        </div>
                    </div>

                </main>
            </form>
            <SimpleAlert
                isOpen={alert.show}
                message={alert.message}
                type={alert.type}
                onClose={() => setAlert({ ...alert, show: false })}
            />
        </div>
    );
}