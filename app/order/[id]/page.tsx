"use client";

import PaymentResume from "@/components/PaymentResume";
import { useAuth } from "@/context/AuthContext";
import { Order } from "@/types/order";
import { formatCurrency, formatDateBR } from "@/utils/format";
import { getStatusName } from "@/utils/orderStatus";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function OrderDetailsPage() {
    const { id } = useParams();
    const router = useRouter();
    const { loading: authLoading, user } = useAuth();

    const [order, setOrder] = useState<Order | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchOrderDetails = async () => {
            try {
                const token = localStorage.getItem("user_token");
                // Ajuste a URL conforme seu endpoint de detalhes
                const response = await fetch(`http://localhost:5012/api/order/${id}`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (!response.ok) throw new Error("Não foi possível carregar os detalhes do pedido.");

                const data = await response.json();
                console.log(data);
                setOrder(data);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        if (!authLoading && id) fetchOrderDetails();
    }, [authLoading, id]);

    if (loading) return <div className="p-6 text-center">Carregando detalhes...</div>;
    if (error) return <div className="p-6 text-red-600 text-center">{error}</div>;
    if (!order) return <div className="p-6 text-center">Pedido não encontrado.</div>;

    return (
        <div className="max-w-3xl mx-auto p-6">
            {/* Cabeçalho e Botão Voltar */}
            <button
                onClick={() => router.back()}
                className="text-sm text-gray-500 hover:text-blue-600 mb-6 flex items-center gap-2"
            >
                ← Voltar para Meus Pedidos
            </button>

            {/* Container do Grid Responsivo */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 items-start">
                {/* COLUNA DA ESQUERDA: Detalhes dos Itens (Ocupa 2 colunas no desktop) */}
                <div className="lg:col-span-2 bg-white rounded-2xl shadow-sm border p-6">
                    <div className="flex justify-between items-start border-b pb-4 mb-6">
                        <div>
                            <h1 className="text-xl font-bold">Pedido #{order.id}</h1>
                            <p className="text-gray-500 text-sm">{formatDateBR(order.creationDate)}</p>
                        </div>
                        <span className="px-4 py-1 rounded-full bg-blue-50 text-blue-700 font-semibold text-sm">
                            {getStatusName(order.status)}
                        </span>
                    </div>

                    {/* Lista de Produtos */}
                    <div className="space-y-4 mb-8">
                        <h2 className="font-semibold text-gray-700">Itens do Pedido</h2>
                        {order.orderProducts?.map((item) => (
                            <div key={item.productId} className="flex justify-between items-center py-2 border-b border-gray-50 last:border-0">
                                <div>
                                    <p className="font-medium">{item.productName}</p>
                                    <p className="text-sm text-gray-500">{item.quantity}x {formatCurrency(item.unitPrice)}</p>
                                </div>
                                <p className="font-semibold">{formatCurrency(item.total)}</p>
                            </div>
                        ))}
                    </div>

                    {/* Resumo de Valores */}
                    <div className="bg-gray-50 rounded-xl p-4 space-y-2">
                        <div className="flex justify-between text-gray-600">
                            <span>Subtotal</span>
                            <span>{formatCurrency(order.total)}</span>
                        </div>
                        <div className="flex justify-between text-gray-600">
                            <span>Frete</span>
                            <span className="text-green-600">Grátis</span>
                        </div>
                        <div className="flex justify-between text-lg font-bold pt-2 border-t mt-2">
                            <span>Total</span>
                            <span>{formatCurrency(order.total)}</span>
                        </div>
                    </div>
                </div>
                {/* COLUNA DA DIREITA: Informações de Pagamento (Ocupa 1 coluna no desktop) */}
                <aside className="lg:col-span-1 space-y-4 sticky top-6">

                    {order.paymentInfo.map((i) =>
                        <div key={i.id}><PaymentResume info={i}></PaymentResume></div>

                    )}

                    {/* Dica de Segurança ou Ajuda (Opcional) */}
                    <div className="p-4 bg-blue-50 rounded-xl border border-blue-100">
                        <p className="text-xs text-blue-700 leading-relaxed">
                            Se houver qualquer problema com o pagamento, entre em contato com nosso suporte.
                        </p>
                    </div>
                </aside>
            </div>
        </div >
    );
}