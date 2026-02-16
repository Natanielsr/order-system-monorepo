"use client";

import { useRouter } from 'next/navigation';
import { CheckCircle2, ShoppingBag, ArrowRight, Truck } from 'lucide-react';

export default function SuccessPage() {
    const router = useRouter();

    return (
        <div className="min-h-screen bg-slate-50 flex items-center justify-center p-6">
            <div className="max-w-md w-full bg-white rounded-3xl shadow-sm border border-slate-100 p-10 text-center animate-in fade-in zoom-in duration-500">

                {/* Ícone Principal */}
                <div className="flex justify-center mb-8">
                    <div className="relative">
                        <div className="absolute inset-0 bg-green-100 rounded-full animate-ping opacity-25"></div>
                        <div className="relative bg-green-50 p-5 rounded-full">
                            <CheckCircle2 className="w-16 h-16 text-green-500" />
                        </div>
                    </div>
                </div>

                {/* Texto de Confirmação */}
                <h1 className="text-2xl font-black text-slate-900 mb-3">
                    Pedido Recebido!
                </h1>
                <p className="text-slate-500 mb-10 leading-relaxed">
                    Sua compra foi processada com sucesso. Em breve você receberá um e-mail com os detalhes do rastreamento.
                </p>

                {/* Card de Informação Rápida */}
                <div className="bg-slate-50 rounded-2xl p-4 mb-10 flex items-center gap-4 text-left">
                    <div className="bg-white p-2 rounded-lg shadow-sm">
                        <Truck className="w-5 h-5 text-blue-500" />
                    </div>
                    <div>
                        <p className="text-xs text-slate-400 uppercase font-bold tracking-wider">Status</p>
                        <p className="text-sm font-semibold text-slate-700">Preparando para envio</p>
                    </div>
                </div>

                {/* Botões de Ação */}
                <div className="grid gap-3">
                    <button
                        onClick={() => router.push('/')}
                        className="w-full bg-slate-900 hover:bg-slate-800 text-white font-bold py-4 rounded-2xl transition-all active:scale-95 flex items-center justify-center gap-2"
                    >
                        <ShoppingBag className="w-5 h-5" />
                        Voltar para a Loja
                    </button>

                    <button
                        onClick={() => router.push('/order')}
                        className="w-full bg-white border border-slate-200 hover:bg-slate-50 text-slate-600 font-bold py-4 rounded-2xl transition-all flex items-center justify-center gap-2"
                    >
                        Acompanhar Pedido
                        <ArrowRight className="w-5 h-5" />
                    </button>
                </div>

            </div>
        </div>
    );
}