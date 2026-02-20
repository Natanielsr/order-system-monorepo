"use client";

import React, { useState, useEffect } from 'react';
import { API_CONFIG } from '@/config/api';
import { useAuth } from '@/context/AuthContext';
import { Address } from '@/types/address';

interface AddressSelectorProps {
    onSelect: (address: Address) => void;
}

export default function AddressSelector({ onSelect }: AddressSelectorProps) {
    const [addresses, setAddresses] = useState<Address[]>([]);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(1);
    const { user, getToken } = useAuth();
    const pageSize = 5;


    const fetchAddresses = async () => {
        setLoading(true);
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
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchAddresses();
    }, [page]);

    // Função para lidar com a mudança de seleção
    const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedId = e.target.value;
        // Encontramos o objeto completo dentro do nosso array local
        const selectedAddress = addresses.find(addr => addr.id === selectedId);

        if (selectedAddress) {
            onSelect(selectedAddress);
        }
    };

    return (
        <div className="flex flex-col gap-1.5 w-full max-w-sm">
            <label htmlFor="address-select" className="text-sm font-medium text-gray-700">
                Selecione o Endereço
            </label>

            <select
                required
                id="address-select"
                defaultValue=""
                disabled={loading}
                onChange={handleChange}
                className="block w-full p-2.5 bg-white border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100 italic"
            >
                <option value="" disabled hidden>
                    {loading ? "Carregando..." : "Clique para selecionar..."}
                </option>

                {addresses.map((addr) => (
                    <option key={addr.id} value={addr.id}>
                        {addr.fullName} - {addr.cpf} - {addr.street} - {addr.city}
                    </option>
                ))}
            </select>
        </div>
    );
}