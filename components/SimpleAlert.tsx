"use client";

import { useEffect } from 'react';
import { XCircle, CheckCircle2 } from 'lucide-react';

interface SimpleAlertProps {
    message: string;
    isOpen: boolean;
    onClose: () => void;
    type?: 'success' | 'error';
}

export default function SimpleAlert({ message, isOpen, onClose, type = 'success' }: SimpleAlertProps) {

    // Timer para fechar sozinho após 3 segundos
    useEffect(() => {
        if (isOpen) {
            const timer = setTimeout(onClose, 2000);
            return () => clearTimeout(timer);
        }
    }, [isOpen, onClose]);

    if (!isOpen) return null;

    return (
        <div className="fixed top-5 left-1/2 -translate-x-1/2 z-9999 animate-bounce-in">
            <div className={`flex items-center gap-3 px-4 py-3 rounded-lg shadow-2xl border ${type === 'success'
                ? 'bg-green-600 border-green-700 text-white'
                : 'bg-red-600 border-red-700 text-white'
                }`}>
                {type === 'success' ? <CheckCircle2 size={20} /> : <XCircle size={20} />}

                <span className="text-sm font-semibold whitespace-pre-line">
                    {message}
                </span>

                <button onClick={onClose} className="ml-2 hover:opacity-70 transition-opacity">
                    <XCircle size={16} />
                </button>
            </div>
        </div>
    );
}