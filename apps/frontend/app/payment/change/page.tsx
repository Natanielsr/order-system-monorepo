"use client";

import { useRouter } from 'next/navigation';
import { ChevronLeft, Plus, ShieldCheck } from 'lucide-react';

export default function ChangePaymentPage() {
    const router = useRouter();

    return (
        <div className="min-h-screen bg-white pb-12">
            {/* Header */}
            <header className="border-b border-gray-200 py-4 mb-8">
                <div className="max-w-3xl mx-auto px-4">
                    <h1 className="text-2xl font-semibold text-gray-800">Selecione uma forma de pagamento</h1>
                </div>
            </header>

            <main className="max-w-3xl mx-auto px-4">
                {/* Voltar */}
                <button onClick={() => router.back()} className="flex items-center text-sm text-blue-600 hover:text-orange-700 hover:underline mb-8">
                    <ChevronLeft className="w-4 h-4" /> Voltar para o checkout
                </button>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-8">

                    {/* Coluna da Esquerda: Opções de Pagamento */}
                    <div className="md:col-span-2 space-y-6">

                        {/* Seção: Seus cartões salvos */}
                        <section>
                            <h3 className="text-lg font-bold border-b border-gray-200 pb-2 mb-4">Seus cartões de crédito e débito</h3>

                            <div className="space-y-3">
                                {/* Cartão 1 (Selecionado) */}
                                <label className="flex items-center p-4 border border-orange-500 bg-orange-50 rounded-lg cursor-pointer transition-all">
                                    <input type="radio" name="payment" defaultChecked className="w-4 h-4 text-orange-600 focus:ring-orange-500" />
                                    <div className="ml-4 flex-1">
                                        <div className="flex items-center gap-2">
                                            <span className="font-bold text-sm">Visa terminado em 4242</span>
                                            <span className="text-xs text-gray-500 uppercase">João Silva</span>
                                        </div>
                                        <p className="text-xs text-gray-600">Expira em: 12/2028</p>
                                    </div>
                                    <div className="w-10 h-6 bg-white border border-gray-300 rounded flex items-center justify-center text-[10px] font-bold">VISA</div>
                                </label>

                                {/* Cartão 2 */}
                                <label className="flex items-center p-4 border border-gray-200 hover:bg-gray-50 rounded-lg cursor-pointer transition-all">
                                    <input type="radio" name="payment" className="w-4 h-4 text-orange-600 focus:ring-orange-500" />
                                    <div className="ml-4 flex-1">
                                        <div className="flex items-center gap-2">
                                            <span className="font-bold text-sm">Mastercard terminado em 8899</span>
                                        </div>
                                        <p className="text-xs text-gray-600">Expira em: 05/2026</p>
                                    </div>
                                    <div className="w-10 h-6 bg-white border border-gray-300 rounded flex items-center justify-center text-[10px] font-bold italic">mastercard</div>
                                </label>
                            </div>
                        </section>

                        {/* Seção: Outras formas de pagamento */}
                        <section className="pt-4">
                            <h3 className="text-lg font-bold border-b border-gray-200 pb-2 mb-4">Outras formas de pagamento</h3>

                            <div className="space-y-4">
                                <button className="flex items-center gap-3 text-sm text-blue-600 hover:text-orange-700 hover:underline">
                                    <Plus className="w-4 h-4 text-gray-600" />
                                    <CreditCardIcon />
                                    Adicionar um cartão de crédito ou débito
                                </button>

                                <label className="flex items-center p-4 border border-gray-200 rounded-lg cursor-pointer">
                                    <input type="radio" name="payment" className="w-4 h-4 text-orange-600 focus:ring-orange-500" />
                                    <div className="ml-4">
                                        <span className="font-bold text-sm">Pix</span>
                                        <p className="text-xs text-gray-500">O código QR será gerado após a confirmação do pedido.</p>
                                    </div>
                                </label>
                            </div>
                        </section>
                    </div>

                    {/* Coluna da Direita: Box de Continuação */}
                    <div className="md:col-span-1">
                        <div className="bg-gray-50 border border-gray-200 rounded-xl p-5 sticky top-4">
                            <button className="w-full bg-[#FFD814] hover:bg-[#F7CA00] border border-[#FCD200] rounded-lg py-2 text-sm font-medium shadow-sm mb-4">
                                Continuar
                            </button>
                            <p className="text-[11px] text-gray-600 leading-tight">
                                Você poderá revisar este pedido antes que ele seja finalizado.
                            </p>

                            <div className="mt-6 pt-6 border-t border-gray-200">
                                <div className="flex items-start gap-2">
                                    <ShieldCheck className="w-8 h-8 text-green-600" />
                                    <p className="text-[11px] text-gray-500">
                                        Sua segurança é nossa prioridade. Não compartilhamos seus dados de pagamento com terceiros.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </main>
        </div>
    );
}

// Pequeno componente de ícone para o botão de adicionar
function CreditCardIcon() {
    return (
        <div className="flex gap-0.5">
            <div className="w-6 h-4 bg-gray-200 rounded-sm"></div>
            <div className="w-6 h-4 bg-gray-200 rounded-sm"></div>
            <div className="w-6 h-4 bg-gray-200 rounded-sm"></div>
        </div>
    );
}