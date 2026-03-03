"use client";

import React, { useEffect, useState } from 'react';
import { Lock, Mail, ArrowRight } from 'lucide-react';
import { useRouter } from 'next/navigation';
import SimpleAlert from '@/components/SimpleAlert';
import { useAuth } from '@/context/AuthContext';
import { useCart } from '@/context/CartContext';

export default function LoginPage() {
    const [alert, setAlert] = useState({ show: false, message: '', type: 'success' as 'success' | 'error' });

    const router = useRouter();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const { closeCart } = useCart();

    useEffect(() => {
        closeCart();
    });

    const { login } = useAuth();

    const showLoginErrorAlert = (message: string) => {
        setAlert({ show: true, message: message, type: 'error' });
    };

    async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault(); // Impede o recarregamento da página
        setLoading(true);
        setError("");

        // Captura os dados do formulário de forma simples
        const formData = new FormData(event.currentTarget);
        const email = formData.get("email");
        const password = formData.get("password");

        // --- CÓDIGO PARA SIMULAR ESPERA (2 segundos) ---
        await new Promise(resolve => setTimeout(resolve, 2000));
        // -----------------------------------------------

        try {
            const response = await fetch('http://localhost:5012/api/auth/loginemail', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email,
                    password
                }),
            });

            if (!response.ok) {
                // 1. Pega o texto bruto primeiro para não perder o stream
                const rawText = await response.text();
                console.log("Status Code:", response.status); // Verifique se é 400, 401, 404 ou 500
                console.log("Resposta Bruta do Servidor:", rawText);

                let errorData;
                try {
                    errorData = JSON.parse(rawText);
                } catch (e) {
                    errorData = { message: rawText };
                }

                // Use strings separadas para evitar que o console agrupe errado
                if (response.status)
                    console.log("ERRO STATUS:", response.status);
                console.log("ERRO MSG:", errorData);
                showLoginErrorAlert("E-mail ou Senha Inválidos!");
                setError(errorData.message);
                return;
            }

            const data = await response.json();

            // 1. Salvar o Token (Exemplo no LocalStorage ou Cookie)
            login(data.token);

            // 2. Redirecionar para o Checkout ou Home
            router.push('/checkout');

        } catch (err: any) {
            setError(err.message);
            showLoginErrorAlert("Não foi possível se conectar ao servidor!");
            console.error(err.message);

        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="w-screen h-screen bg-gray-50 flex items-center justify-center p-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-xl p-8">

                {/* Header */}
                <div className="text-center mb-8">
                    <h2 className="text-3xl font-bold text-gray-900">Login</h2>
                    <p className="text-gray-500 mt-2">Insira seus dados para acessar a conta</p>
                </div>

                {/* Formulário */}
                <form onSubmit={handleSubmit} className="space-y-6">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">E-mail</label>
                        <div className="relative">
                            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <Mail className="h-5 w-5 text-gray-400" />
                            </div>
                            <input
                                disabled={loading}
                                name='email'
                                type="email"
                                className="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                                placeholder="seu@email.com"
                                required
                            />
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Senha</label>
                        <div className="relative">
                            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <Lock className="h-5 w-5 text-gray-400" />
                            </div>
                            <input
                                disabled={loading}
                                name='password'
                                type="password"
                                className="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
                                placeholder="••••••••"
                                required
                            />
                        </div>
                    </div>

                    <div className="flex items-center justify-between text-sm">
                        <label className="flex items-center text-gray-600">
                            <input disabled={loading} type="checkbox" className="rounded border-gray-300 text-blue-600 mr-2" />
                            Lembrar de mim
                        </label>
                        <a href="#" className="text-blue-600 hover:underline font-medium">Esqueceu a senha?</a>
                    </div>

                    <button
                        disabled={loading}
                        type="submit"
                        className={`w-full text-white font-semibold py-3 rounded-lg flex items-center justify-center gap-2 transition-colors group
                        ${loading
                                ? 'bg-gray-400 cursor-not-allowed scale-95' // Cor quando desativado
                                : 'bg-blue-600 hover:bg-blue-700 active:scale-95 shadow-md' // Cor quando ativo
                            }
                        `}
                    >
                        {loading ? "Entrando..." :
                            <>Entrar<ArrowRight className="h-4 w-4 group-hover:translate-x-1 transition-transform" /></>
                        }
                    </button>
                </form>

                {/* Rodapé */}
                <p className="text-center mt-8 text-sm text-gray-600">
                    Não tem uma conta?{' '}
                    <a href="/register" className="text-blue-600 hover:underline font-bold">Cadastre-se</a>
                </p>
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