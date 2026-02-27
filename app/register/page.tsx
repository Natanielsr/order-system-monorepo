"use client";

import SimpleAlert from '@/components/SimpleAlert';
import { API_CONFIG } from '@/config/api';
import { User } from '@/types/user';
import { validatePassword } from '@/utils/validation';
import { useRouter } from 'next/navigation';
import React, { useState } from 'react';

export default function MaterialRegister() {
    const [loading, setLoading] = useState(false);
    const [alert, setAlert] = useState({ show: false, message: '', type: 'success' as 'success' | 'error' });
    const router = useRouter();

    // Estado do formulário
    const [formData, setFormData] = useState<User>({
        username: '',
        email: '',
        password: '',
        confirmPassword: ''
    });

    // Função para parsear erros da API
    const parseApiError = (errorMessage: string): string => {
        try {
            const errorJson = JSON.parse(errorMessage);
            if (Array.isArray(errorJson.errors)) {
                return errorJson.errors.join("\n");
            }
        } catch {
            // Se não conseguir parsear, retorna a mensagem original
        }
        return errorMessage || "Não foi possível criar a conta";
    };

    // Envio para a API
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);

        // Validação
        var validatePasswordResponse = validatePassword(formData.password, formData.confirmPassword);
        if (!validatePasswordResponse.isValid) {

            var errorsStr: string = validatePasswordResponse.errors.join("\n ");
            setAlert({ show: true, message: errorsStr, type: "error" });
            setLoading(false);
            return;
        }

        try {
            const response = await fetch(`${API_CONFIG.BASE_URL}/user`, {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });

            const errorMessage = await response.text();
            if (!response.ok) {
                const errorsStr = parseApiError(errorMessage);
                setAlert({ show: true, message: errorsStr, type: 'error' });
                return;
            }

            setAlert({ show: true, message: 'Conta criada com sucesso!', type: 'success' });
            router.push("/login");
        } catch (error: any) {
            console.error("Erro ao criar usuário:", error);
            setAlert({ show: true, message: "Erro na requisição. Tente novamente.", type: 'error' });
        } finally {
            setLoading(false);
        }
    };

    // Handler para inputs
    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value, type } = e.target;
        let displayValue = value;

        setFormData(prev => ({ ...prev, [name]: displayValue }));

    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-[#f7f9fc] p-4 text-slate-900">

            <main className="w-full max-w-md bg-white p-8 rounded-[28px] shadow-sm border border-slate-100">

                {/* Header Material */}
                <div className="flex flex-col items-center mb-8">
                    <div className="w-12 h-12 bg-blue-600 rounded-xl flex items-center justify-center mb-4 shadow-md">
                        <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                        </svg>
                    </div>
                    <h1 className="text-2xl font-normal text-slate-900">Criar conta</h1>
                    <p className="text-slate-500 text-sm mt-1">Use seu e-mail para começar</p>
                </div>

                <form onSubmit={handleSubmit} className="space-y-4">
                    {/* Campo de Input Estilo Material (Outlined) */}
                    <div className="relative">
                        <input
                            required
                            type="text"
                            name='username'
                            value={formData.username}
                            onChange={handleChange}
                            id="username"
                            placeholder=" "
                            className="block w-full px-4 py-4 text-base bg-transparent border border-slate-400 rounded-lg appearance-none focus:outline-none focus:ring-0 focus:border-blue-600 peer"
                        />
                        <label
                            htmlFor="username"
                            className="absolute text-slate-500 duration-300 transform -translate-y-4 scale-75 top-2 z-10 origin-left bg-white px-2 peer-focus:px-2 peer-focus:text-blue-600 peer-placeholder-shown:scale-100 peer-placeholder-shown:-translate-y-1/2 peer-placeholder-shown:top-1/2 peer-focus:top-2 peer-focus:scale-75 peer-focus:-translate-y-4 left-3"
                        >
                            Nome de Usuário
                        </label>
                    </div>

                    <div className="relative">
                        <input
                            required
                            type="email"
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                            id="email"
                            placeholder=" "
                            className="block w-full px-4 py-4 text-base bg-transparent border border-slate-400 rounded-lg appearance-none focus:outline-none focus:ring-0 focus:border-blue-600 peer"
                        />
                        <label
                            htmlFor="email"
                            className="absolute text-slate-500 duration-300 transform -translate-y-4 scale-75 top-2 z-10 origin-left bg-white px-2 peer-focus:px-2 peer-focus:text-blue-600 peer-placeholder-shown:scale-100 peer-placeholder-shown:-translate-y-1/2 peer-placeholder-shown:top-1/2 peer-focus:top-2 peer-focus:scale-75 peer-focus:-translate-y-4 left-3"
                        >
                            E-mail
                        </label>
                    </div>

                    <div className="relative">
                        <input
                            required
                            type="password"
                            name='password'
                            value={formData.password}
                            onChange={handleChange}
                            id="password"
                            placeholder=" "
                            className="block w-full px-4 py-4 text-base bg-transparent border border-slate-400 rounded-lg appearance-none focus:outline-none focus:ring-0 focus:border-blue-600 peer"
                        />
                        <label
                            htmlFor="password"
                            className="absolute text-slate-500 duration-300 transform -translate-y-4 scale-75 top-2 z-10 origin-left bg-white px-2 peer-focus:px-2 peer-focus:text-blue-600 peer-placeholder-shown:scale-100 peer-placeholder-shown:-translate-y-1/2 peer-placeholder-shown:top-1/2 peer-focus:top-2 peer-focus:scale-75 peer-focus:-translate-y-4 left-3"
                        >
                            Senha
                        </label>
                    </div>

                    <div className="relative">
                        <input
                            required
                            name='confirmPassword'
                            value={formData.confirmPassword}
                            onChange={handleChange}
                            type="password"
                            id="confirmPassword"
                            placeholder=" "
                            className="block w-full px-4 py-4 text-base bg-transparent border border-slate-400 rounded-lg appearance-none focus:outline-none focus:ring-0 focus:border-blue-600 peer"
                        />
                        <label
                            htmlFor="confirmPassword"
                            className="absolute text-slate-500 duration-300 transform -translate-y-4 scale-75 top-2 z-10 origin-left bg-white px-2 peer-focus:px-2 peer-focus:text-blue-600 peer-placeholder-shown:scale-100 peer-placeholder-shown:-translate-y-1/2 peer-placeholder-shown:top-1/2 peer-focus:top-2 peer-focus:scale-75 peer-focus:-translate-y-4 left-3"
                        >
                            Confirmar Senha
                        </label>
                    </div>

                    {/* Botão de Ação Primária (Filled Button) */}
                    <button className={`w-full text-white font-semibold py-3 rounded-lg flex items-center justify-center gap-2 transition-colors group
                        ${loading
                            ? 'bg-gray-400 cursor-not-allowed scale-95' // Cor quando desativado
                            : 'bg-blue-600 hover:bg-blue-700 active:scale-95 shadow-md' // Cor quando ativo
                        }
                        `}>
                        Cadastrar
                    </button>
                </form>

                <div className="mt-8 flex flex-col items-center space-y-4">
                    <button className="text-blue-600 text-sm font-medium hover:bg-blue-50 px-4 py-2 rounded-full transition-colors">
                        Já tem uma conta? Entrar
                    </button>
                </div>

            </main>
            <SimpleAlert
                isOpen={alert.show}
                message={alert.message}
                type={alert.type}
                onClose={() => setAlert({ ...alert, show: false })}
            />
        </div>
    );
}