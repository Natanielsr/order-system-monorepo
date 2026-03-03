import { PaymentInfo } from "@/types/paymentInfo";
import { formatCurrency } from "@/utils/format";

interface PaymentResumeProps {
    info: PaymentInfo
}

export default function PaymentResume({ info }: PaymentResumeProps) {
    return <div className="bg-white rounded-2xl shadow-sm border p-6">
        <h2 className="font-bold text-lg mb-4 text-slate-800 border-b pb-2">Resumo de Pagamento</h2>

        <div className="space-y-3">
            {/* Detalhes do Método */}
            <div className="flex justify-between items-center py-2">
                <span className="text-gray-500 text-sm">Método:</span>
                {info.method == 0 ? <span className="font-medium text-slate-700 uppercase">Pix</span> : <></>}
                {info.method == 1 ? <span className="font-medium text-slate-700 uppercase">Cartão</span> : <></>}
                {info.method == 2 ? <span className="font-medium text-slate-700 uppercase">Boleto</span> : <></>}

                {/* ^ Aqui você pode trocar pelo valor vindo do banco 'order.paymentMethod' */}
            </div>

            <div className="flex justify-between items-center py-2">
                <span className="text-gray-500 text-sm">Status:</span>
                {info.status == 0 ? <span className="text-sm font-semibold text-yellow-600 bg-green-50 px-2 py-0.5 rounded">Pendente</span> : <></>}
                {info.status == 1 ? <span className="text-sm font-semibold text-blue-600 bg-green-50 px-2 py-0.5 rounded">Autorizado</span> : <></>}
                {info.status == 2 ? <span className="text-sm font-semibold text-green-600 bg-green-50 px-2 py-0.5 rounded">Aprovado</span> : <></>}
                {info.status == 3 ? <span className="text-sm font-semibold text-gray-600 bg-green-50 px-2 py-0.5 rounded">Reembolsado</span> : <></>}
                {info.status == 4 ? <span className="text-sm font-semibold text-red-600 bg-green-50 px-2 py-0.5 rounded">Cancelado</span> : <></>}
            </div>

            <hr className="border-gray-50" />

            {/* Valores */}
            <div className="flex justify-between text-gray-600">
                <span>Subtotal</span>
                <span>{formatCurrency(info.paidAmount)}</span>
            </div>
            <div className="flex justify-between text-gray-600">
                <span>Frete</span>
                <span className="text-green-600 font-medium">Grátis</span>
            </div>

            <div className="flex justify-between text-xl font-bold pt-4 text-slate-900 border-t border-gray-100">
                <span>Total</span>
                <span>{formatCurrency(info.paidAmount)}</span>
            </div>
        </div>

        {/* Botão de Ação Opcional (ex: baixar nota ou ver comprovante) */}
        {info.status == 2 ?
            <button className="w-full mt-6 py-3 px-4 bg-slate-800 hover:bg-slate-900 text-white rounded-xl font-medium transition-colors">
                Ver Comprovante
            </button> : <></>
        }

    </div>
}