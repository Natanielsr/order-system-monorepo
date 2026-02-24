"use client";

import { useEffect, useState } from 'react';
import { ChevronLeft, Loader2 } from 'lucide-react';
import { useRouter, useSearchParams } from 'next/navigation';
import { API_CONFIG } from '@/config/api';
import { useAuth } from '@/context/AuthContext';
import SimpleAlert from '@/components/SimpleAlert';
import { maskCEP, maskCPF } from '@/utils/mask';
import Spinner from '@/components/Spinner';
import { Address } from '@/types/address';

export default function AddressPage() {
    const router = useRouter();
    const [loadingAddress, setLoadingAddress] = useState(false);
    const [searchingZipCode, setSearchingZipCode] = useState(false);
    const { user, isAuthenticated, getToken, loading } = useAuth();
    const [alert, setAlert] = useState({ show: false, message: '', type: 'success' as 'success' | 'error' });
    const searchParams = useSearchParams();
    const addressId = searchParams.get('id');
    const [error, setError] = useState(false);

    const ESTADOS_BRASIL = [
        { uf: 'AC', nome: 'Acre' }, { uf: 'AL', nome: 'Alagoas' }, { uf: 'AP', nome: 'Amapá' },
        { uf: 'AM', nome: 'Amazonas' }, { uf: 'BA', nome: 'Bahia' }, { uf: 'CE', nome: 'Ceará' },
        { uf: 'DF', nome: 'Distrito Federal' }, { uf: 'ES', nome: 'Espírito Santo' }, { uf: 'GO', nome: 'Goiás' },
        { uf: 'MA', nome: 'Maranhão' }, { uf: 'MT', nome: 'Mato Grosso' }, { uf: 'MS', nome: 'Mato Grosso do Sul' },
        { uf: 'MG', nome: 'Minas Gerais' }, { uf: 'PA', nome: 'Pará' }, { uf: 'PB', nome: 'Paraíba' },
        { uf: 'PR', nome: 'Paraná' }, { uf: 'PE', nome: 'Pernambuco' }, { uf: 'PI', nome: 'Piauí' },
        { uf: 'RJ', nome: 'Rio de Janeiro' }, { uf: 'RN', nome: 'Rio Grande do Norte' }, { uf: 'RS', nome: 'Rio Grande do Sul' },
        { uf: 'RO', nome: 'Rondônia' }, { uf: 'RR', nome: 'Roraima' }, { uf: 'SC', nome: 'Santa Catarina' },
        { uf: 'SP', nome: 'São Paulo' }, { uf: 'SE', nome: 'Sergipe' }, { uf: 'TO', nome: 'Tocantins' }
    ];

    // Estado do formulário
    const [formData, setFormData] = useState<Address>({
        id: '',
        fullName: '',
        cpf: '',
        zipCode: '',
        street: '',
        number: '',
        neighborhood: '',
        complement: '',
        city: '',
        state: '',
        isDefault: false,
        userId: ''
    });

    useEffect(() => {
        if (!isAuthenticated && !loading) {
            router.push("/login");
            return;
        }

        if (user?.nameid) {
            setFormData(prev => ({
                ...prev,
                userId: user.nameid
            }));
        }

        if (addressId) {

            setFormData(prev => ({
                ...prev,
                id: addressId
            }));

            handleGetAddress();
        }

    }, [isAuthenticated, loading, router, user, addressId ? addressId : null]);

    // Handler para inputs
    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value, type } = e.target;
        let displayValue = value;

        // Aplicando Máscaras
        if (name === 'cpf') {
            displayValue = maskCPF(value);
        } else if (name === 'zipCode') {
            displayValue = maskCEP(value);
            checkCEP(value);
        }

        const checked = (e.target as HTMLInputElement).checked;

        if (type === 'checkbox') {
            const checked = (e.target as HTMLInputElement).checked;
            setFormData(prev => ({ ...prev, [name]: checked }));
        } else {
            setFormData(prev => ({ ...prev, [name]: displayValue }));
        }
    };

    const checkCEP = async (cep: string) => {
        const cleanCEP = cep.replace(/\D/g, "");
        if (cleanCEP.length === 8) {
            console.log("buscando cep...");
            setSearchingZipCode(true);
            try {
                const res = await fetch(`https://viacep.com.br/ws/${cleanCEP}/json/`);
                const data = await res.json();
                if (!data.erro) {
                    setFormData(prev => ({
                        ...prev,
                        street: data.logradouro,
                        neighborhood: data.bairro,
                        city: data.localidade,
                        state: data.uf
                    }));
                }
                setSearchingZipCode(false);
            } catch (err) {
                console.error("Erro ao buscar CEP");
                setSearchingZipCode(false);
            }
        }
    };

    // Envio para a API
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        // Validação de CPF
        if (formData.cpf.replace(/\D/g, "").length !== 11) {
            setAlert({ show: true, message: 'CPF inválido. Insira os 11 dígitos.', type: 'error' });
            return;
        }

        // Validação de CEP
        if (formData.zipCode.replace(/\D/g, "").length !== 8) {
            setAlert({ show: true, message: 'CEP incompleto.', type: 'error' });
            return;
        }


        setLoadingAddress(true);
        var token = getToken();

        var method = "";
        if (addressId)
            method = "PUT";
        else
            method = "POST";

        try {
            const response = await fetch(`${API_CONFIG.BASE_URL}/address`, {
                method: method,
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(formData),
            });
            var responseStr = await response.text();
            if (!response.ok) throw new Error(responseStr);
            setAlert({ show: true, message: 'Endereço adicionado com sucesso!', type: 'success' });
            router.back(); // Redireciona após sucesso
        } catch (error: any) {
            console.error("failed to create address");
            console.error(error.message);
            var errorJson = JSON.parse(error.message);
            var errorsStr = errorJson.errors.map((e: string) => e);

            setAlert({ show: true, message: errorsStr, type: 'error' });
        } finally {
            setLoadingAddress(false);
        }
    };

    const handleGetAddress = async () => {
        try {
            var token = getToken();

            const response = await fetch(`${API_CONFIG.BASE_URL}/address/${addressId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                }
            });

            if (!response.ok) {
                // Tenta pegar o texto do erro, caso não seja JSON
                const errorText = await response.text();
                throw new Error(errorText);
            }
            var data = await response.json();
            setFormData(data);
            console.log(data);

        } catch (error: any) {
            console.error("failed to get address");
            try {
                // 2. Tenta parsear o erro apenas se ele parecer um JSON
                const errorJson = JSON.parse(error.message);
                const errorsStr = errorJson.errors ? errorJson.errors.join(", ") : "Erro desconhecido";
                setAlert({ show: true, message: errorsStr, type: 'error' });
            } catch {
                // 3. Se o erro não for JSON (ex: string simples ou HTML), cai aqui
                setAlert({ show: true, message: error.message || "Erro ao carregar endereço", type: 'error' });
            }
            setError(true);
        } finally {
            setLoadingAddress(false);
        }
    };


    return (
        <div className="min-h-screen bg-white text-gray-900 pb-12">
            <header className="border-b border-gray-200 py-4 mb-8">
                <div className="max-w-3xl mx-auto px-4">
                    <h1 className="text-2xl font-semibold">
                        {addressId ? "Alterar Endereço" : "Adicionar um novo endereço"}

                    </h1>
                </div>
            </header>

            <main className="max-w-xl mx-auto px-4">
                <button onClick={() => router.back()} className="flex items-center text-sm text-blue-600 hover:text-orange-700 hover:underline mb-6">
                    <ChevronLeft className="w-4 h-4" /> Voltar
                </button>

                <form onSubmit={handleSubmit} className="space-y-5">
                    {/* Nome Completo */}
                    <div>
                        <label className="block text-sm font-bold mb-1">Nome completo (Nome e Sobrenome)</label>
                        <input
                            required
                            name="fullName"
                            value={formData.fullName}
                            onChange={handleChange}
                            type="text"
                            className="w-full p-2 border border-gray-400 rounded-md focus:border-orange-500 focus:ring-1 focus:ring-orange-500 outline-none"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-bold mb-1">CPF</label>
                        <input
                            required
                            name="cpf"
                            placeholder="999.999.999-99"
                            value={formData.cpf}
                            onChange={handleChange}
                            type="text"
                            className="w-full p-2 border border-gray-400 rounded-md focus:border-orange-500 focus:ring-1 focus:ring-orange-500 outline-none"
                        />
                    </div>

                    {/* CEP */}
                    <div>
                        <label className="block text-sm font-bold mb-1">CEP</label>
                        <div className='flex'>
                            <input
                                required
                                name="zipCode"
                                value={formData.zipCode}
                                onChange={handleChange}
                                type="text"
                                placeholder="00000-000"
                                className="w-full md:w-1/2 p-2 border border-gray-400 rounded-md focus:border-orange-500 focus:ring-1 focus:ring-orange-500 outline-none"
                            />
                            {searchingZipCode && (
                                <div className='m-1'>
                                    <Spinner></Spinner>
                                </div>
                            )}

                        </div>

                    </div>

                    {/* Endereço */}
                    <div>
                        <label className="block text-sm font-bold mb-1">Endereço</label>
                        <input
                            required
                            name="street"
                            value={formData.street}
                            onChange={handleChange}
                            type="text"
                            placeholder="Nome da rua"
                            className="w-full p-2 border border-gray-400 rounded-md focus:border-orange-500 focus:ring-1 focus:ring-orange-500 outline-none mb-2"
                        />
                        <label className="block text-sm font-bold mb-1">Numero</label>
                        <input
                            required
                            name="number"
                            value={formData.number}
                            onChange={handleChange}
                            type="text"
                            placeholder="Numero"
                            className="w-full md:w-1/2 p-2 border border-gray-400 rounded-md focus:border-orange-500 focus:ring-1 focus:ring-orange-500 outline-none mb-2"
                        />
                        <label className="block text-sm font-bold mb-1">Complemento</label>
                        <input
                            name="complement"
                            value={formData.complement}
                            onChange={handleChange}
                            type="text"
                            placeholder="Apartamento, suíte, etc."
                            className="w-full p-2 border border-gray-400 rounded-md focus:border-orange-500 focus:ring-1 focus:ring-orange-500 outline-none"
                        />
                        <label className="block text-sm font-bold mb-1">Bairro</label>
                        <input
                            required
                            name="neighborhood"
                            value={formData.neighborhood}
                            onChange={handleChange}
                            type="text"
                            placeholder="Vila Ribeiro"
                            className="w-full p-2 border border-gray-400 rounded-md focus:border-orange-500 focus:ring-1 focus:ring-orange-500 outline-none"
                        />
                    </div>

                    {/* Cidade e Estado */}
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div>
                            <label className="block text-sm font-bold mb-1">Cidade</label>
                            <input required name="city" value={formData.city} onChange={handleChange} type="text" className="w-full p-2 border border-gray-400 rounded-md focus:border-orange-500 focus:ring-1 focus:ring-orange-500 outline-none" />
                        </div>
                        <div>
                            <label className="block text-sm font-bold mb-1">Estado</label>
                            <select
                                required
                                name="state"
                                value={formData.state}
                                onChange={handleChange}
                                className="w-full p-2 border border-gray-400 rounded-md bg-white focus:border-orange-500 focus:ring-1 focus:ring-orange-500 outline-none transition-shadow appearance-none"
                            >
                                <option value="" disabled>Selecione</option>
                                {ESTADOS_BRASIL.map((estado) => (
                                    <option key={estado.uf} value={estado.uf}>
                                        {estado.nome}
                                    </option>
                                ))}
                            </select>
                        </div>
                    </div>

                    {/* Checkbox Padrão */}
                    <div className="flex items-center gap-2 pt-2">
                        <input
                            type="checkbox"
                            id="default"
                            name="isDefault"
                            checked={formData.isDefault ?? false}
                            onChange={handleChange}
                            className="w-4 h-4 rounded border-gray-300 text-orange-600 focus:ring-orange-500"
                        />
                        <label htmlFor="default" className="text-sm">Tornar este meu endereço padrão</label>
                    </div>

                    {/* Botão de Envio */}
                    <div className="pt-4">
                        <button
                            type="submit"
                            disabled={loading || error || loadingAddress}
                            className="w-full md:w-auto bg-[#FFD814] hover:bg-[#F7CA00] border border-[#FCD200] rounded-lg py-2 px-10 text-sm font-medium shadow-sm transition-colors flex items-center justify-center gap-2"
                        >
                            {loading && <Loader2 className="w-4 h-4 animate-spin" />}
                            {loading ? 'Salvando...' : 'Salvar endereço'}
                        </button>
                    </div>
                </form>
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