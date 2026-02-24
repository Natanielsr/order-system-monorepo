"use client";

import React, { useEffect, useState } from 'react';
import { User, Mail, Phone, Shield, Save, Edit2, X, MapPin } from 'lucide-react';
import { useAuth } from '@/context/AuthContext';
import { API_CONFIG } from '@/config/api';
import { Address } from '@/types/address';
import AddressCard from '@/components/AddressCard';
import { useRouter } from 'next/navigation';
import Spinner from '@/components/Spinner';

export default function UserProfile() {
    const [isEditing, setIsEditing] = useState(false);
    const { user, getToken, loading, isAuthenticated } = useAuth();
    const [addresses, setAddresses] = useState<Address[]>([]);
    const router = useRouter();
    const page = 1;
    const pageSize = 5;


    const [userData, setUserData] = useState({
        name: "",
        email: "",
        phone: "(11) 98765-4321",
        role: "",
        bio: "Desenvolvedor focado em soluções escaláveis e UX."
    });

    const handleSave = (e: React.FormEvent) => {
        e.preventDefault();
        setIsEditing(false);
        // Aqui você faria a chamada para sua API (ex: fetch('/api/user', { method: 'PUT' }))
        console.log("Dados salvos:", userData);
    };

    const fetchAddresses = async () => {
        try {
            var token = getToken();
            // Ajuste a URL para o seu endpoint real
            const response = await fetch(
                `${API_CONFIG.BASE_URL}/address/GetUserAddresses?userId=${user?.nameid}&page=${page}&pageSize=${pageSize}`,
                {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}` // Se necessário
                    },
                }
            );
            const data = await response.json();
            console.log(data);
            setAddresses(data);
        } catch (error) {
            console.error("Erro ao buscar endereços:", error);
        } finally {
        }
    };

    useEffect(() => {
        if (!isAuthenticated && !loading) {
            router.push("/login");

            return;
        }

        if (user) {
            setUserData(prev => ({
                ...prev,
                name: user.unique_name || "",
                email: user.email || "",
                role: user.role || ""
            }));
        }

        if (loading == false)
            fetchAddresses();
    }, [loading, user]);

    const handleDeleteAddress = () => {
        fetchAddresses();
    }

    if (!isAuthenticated) {
        return <div><Spinner></Spinner></div>
    }

    return (
        <div className="max-w-4xl mx-auto p-6">
            <div className="bg-white shadow-md rounded-xl border border-gray-100 overflow-hidden">

                {/* Header do Perfil */}
                <div className="p-8 bg-gray-50 border-b border-gray-100 flex flex-col md:flex-row justify-between items-center gap-4">
                    <div className="flex items-center gap-5">
                        <div className="w-20 h-20 bg-blue-600 rounded-full flex items-center justify-center text-white text-3xl font-bold">
                            {userData.name ? userData.name.charAt(0).toUpperCase() : "undefined"}
                        </div>
                        <div>
                            <h1 className="text-2xl font-bold text-gray-800">{userData.name}</h1>
                            <p className="text-gray-500 flex items-center gap-1">
                                <Shield size={14} /> {userData.role}
                            </p>
                        </div>
                    </div>

                    <button
                        onClick={() => setIsEditing(!isEditing)}
                        className={`flex items-center gap-2 px-4 py-2 rounded-lg font-medium transition-all ${isEditing
                            ? "bg-gray-200 text-gray-700 hover:bg-gray-300"
                            : "bg-blue-600 text-white hover:bg-blue-700"
                            }`}
                    >
                        {isEditing ? <><X size={18} /> Cancelar</> : <><Edit2 size={18} /> Editar Perfil</>}
                    </button>
                </div>

                {/* Formulário / Dados */}
                <form onSubmit={handleSave} className="p-8">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

                        {/* Campo: Nome */}
                        <div className="space-y-2">
                            <label className="text-sm font-semibold text-gray-600 flex items-center gap-2">
                                <User size={16} /> Nome Completo
                            </label>
                            <input
                                disabled={!isEditing}
                                className="w-full p-2.5 border rounded-lg bg-white disabled:bg-gray-50 disabled:text-gray-500 focus:ring-2 focus:ring-blue-500 outline-none transition-all"
                                value={userData.name}
                                onChange={(e) => setUserData({ ...userData, name: e.target.value })}
                            />
                        </div>

                        {/* Campo: Email */}
                        <div className="space-y-2">
                            <label className="text-sm font-semibold text-gray-600 flex items-center gap-2">
                                <Mail size={16} /> E-mail
                            </label>
                            <input
                                disabled={!isEditing}
                                type="email"
                                className="w-full p-2.5 border rounded-lg bg-white disabled:bg-gray-50 disabled:text-gray-500 focus:ring-2 focus:ring-blue-500 outline-none transition-all"
                                value={userData.email}
                                onChange={(e) => setUserData({ ...userData, email: e.target.value })}
                            />
                        </div>

                        {/* Campo: Telefone */}
                        <div className="space-y-2">
                            <label className="text-sm font-semibold text-gray-600 flex items-center gap-2">
                                <Phone size={16} /> Telefone
                            </label>
                            <input
                                disabled={!isEditing}
                                className="w-full p-2.5 border rounded-lg bg-white disabled:bg-gray-50 disabled:text-gray-500 focus:ring-2 focus:ring-blue-500 outline-none transition-all"
                                value={userData.phone}
                                onChange={(e) => setUserData({ ...userData, phone: e.target.value })}
                            />
                        </div>
                    </div>

                    {/* Botão Salvar (Apenas no modo edição) */}
                    {isEditing && (
                        <div className="mt-8 flex justify-end">
                            <button
                                type="submit"
                                className="flex items-center gap-2 bg-green-600 text-white px-6 py-2.5 rounded-lg font-semibold hover:bg-green-700 transition-colors shadow-sm"
                            >
                                <Save size={18} /> Salvar Alterações
                            </button>
                        </div>
                    )}
                </form>
                {/* Seção de Endereços */}
                <div className="p-8 bg-gray-50 border-t border-gray-100">
                    <div className="flex justify-between items-center mb-6">
                        <h2 className="text-xl font-bold text-gray-800 flex items-center gap-2">
                            <MapPin size={20} className="text-blue-600" /> Meus Endereços
                        </h2>
                        <button onClick={() => { router.push("/address"); }} className="cursor-pointer text-sm font-semibold text-blue-600 hover:underline">
                            + Adicionar novo
                        </button>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        {/* Card de Endereço */}
                        {addresses?.map((a) => (
                            <AddressCard key={a.id} address={a} onDelete={handleDeleteAddress}></AddressCard>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
}