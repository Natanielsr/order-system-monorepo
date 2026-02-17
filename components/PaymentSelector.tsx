"use client";
import { useState } from "react";
import { CreditCard, QrCode, Barcode } from "lucide-react"; // Opcional: biblioteca de ícones

// Definimos o que o componente aceita
interface PaymentSelectorProps {
    value: number;
    onChange: (newValue: number) => void;
}

const PaymentSelector = ({ value, onChange }: PaymentSelectorProps) => {
    const options = [
        { id: 0, label: "Pix", icon: <QrCode size={20} /> },
        { id: 1, label: "Cartão de Crédito", icon: <CreditCard size={20} /> },
        { id: 2, label: "Boleto", icon: <Barcode size={20} /> },
    ];

    return (
        <div className="flex flex-col gap-4 p-6 bg-white rounded-xl shadow-sm border border-slate-200 w-full max-w-md">
            <div className="grid grid-cols-3 gap-3">
                {options.map((opt) => (
                    <button
                        key={opt.id}
                        onClick={() => onChange(opt.id)}
                        className={`flex flex-col items-center justify-center gap-2 p-4 rounded-lg border-2 transition-all
              ${value === opt.id
                                ? "border-blue-600 bg-blue-50 text-blue-600"
                                : "border-slate-100 bg-slate-50 text-slate-500 hover:border-slate-200"
                            }`}
                    >
                        {opt.icon}
                        <span className="text-sm font-medium">{opt.label}</span>
                    </button>
                ))}
            </div>
        </div>
    );
};

export default PaymentSelector;