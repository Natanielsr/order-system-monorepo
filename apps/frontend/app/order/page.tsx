"use client";

import { useAuth } from "@/context/AuthContext";
import { Order } from "@/types/order";
import { formatCurrency, formatDateBR } from "@/utils/format";
import { getStatusColor, getStatusName } from "@/utils/orderStatus";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useCallback, useEffect, useState } from "react";

export default function OrdersPage() {

    const { loading: authLoading, user, isAuthenticated, getToken } = useAuth();

    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const router = useRouter();

    // ESTADOS PARA PAGINAÇÃO
    const [currentPage, setCurrentPage] = useState(1);
    const pageSize = 5;

    const fetchOrders = useCallback(async (page: number) => {
        setLoading(true);
        try {
            const token = getToken();

            const response = await fetch(`http://localhost:5012/api/order/getuserorders?userId=${user?.nameid}&page=${page}&pageSize=${pageSize}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (!response.ok) {
                console.error(`Error ${response.status}: ${response.statusText}`);
                throw new Error("Error fetch Orders");
            }

            const data = await response.json();
            console.log(data);
            setOrders(data);
        } catch (err: any) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }, [user?.nameid]);

    useEffect(() => {
        if (!isAuthenticated) {
            router.push("/login");
            return;
        }

        if (!authLoading && user?.nameid) {
            fetchOrders(currentPage);
        }


    }, [authLoading, user?.nameid, currentPage, fetchOrders]);

    // Funções de navegação
    const goToNextPage = () => setCurrentPage((prev) => prev + 1);
    const goToPrevPage = () => setCurrentPage((prev) => Math.max(prev - 1, 1));

    if (loading) {
        return <div className="p-6">Carregando pedidos...</div>;
    }

    if (error) {
        return <div className="p-6 text-red-600">{error}</div>;
    }


    return (
        <div className="max-w-4xl mx-auto p-6">
            <h1 className="text-2xl font-bold mb-6">Meus Pedidos</h1>

            <div className="space-y-4">
                {orders.map((order) => (
                    <div
                        key={order.id}
                        className="bg-white rounded-xl shadow-md p-5 border hover:shadow-lg transition-shadow"
                    >
                        <div className="flex justify-between items-center mb-3">
                            <div>
                                <p className="text-sm text-gray-500">
                                    Pedido #{order.code}
                                </p>
                                <p className="text-sm text-gray-500">
                                    Data: {formatDateBR(order.creationDate)}
                                </p>
                            </div>

                            <span
                                className={`px - 3 py - 1 rounded - full text - xs font - medium ${getStatusColor(
                                    order.status
                                )
                                    }`}
                            >
                                {getStatusName(order.status)}
                            </span>
                        </div>

                        <div className="flex justify-between items-center">
                            <p className="font-semibold text-lg">
                                {formatCurrency(order.total)}
                            </p>

                            <Link
                                href={`/order/${order.id}`}
                                className="text-sm text-blue-600 hover:underline"
                            >
                                Ver detalhes
                            </Link>
                        </div>
                    </div>
                ))}
            </div>

            {/* CONTROLES DE PAGINAÇÃO */}
            <div className="flex justify-between items-center mt-8">
                <button
                    onClick={goToPrevPage}
                    disabled={currentPage === 1 || loading}
                    className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50 hover:bg-gray-300 transition-colors"
                >
                    Anterior
                </button>

                <span className="font-medium">Página {currentPage}</span>

                <button
                    onClick={goToNextPage}
                    disabled={orders.length < pageSize || loading}
                    className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50 hover:bg-gray-300 transition-colors"
                >
                    Próxima
                </button>
            </div>

            {orders.length === 0 && (
                <div className="text-center text-gray-500 mt-10">
                    Você ainda não possui pedidos.
                </div>
            )}
        </div>
    );
}
