import { Address } from "@/types/address";
import { Delete, DeleteIcon, Edit2, LucideDelete, RemoveFormatting, RemoveFormattingIcon, Trash } from "lucide-react";
import { useRouter } from "next/navigation";
import ConfirmDeleteModal from "./ConfirmDeleteModal";
import { useState } from "react";
import { API_CONFIG } from "@/config/api";
import SimpleAlert from "./SimpleAlert";
import { useAuth } from "@/context/AuthContext";

interface AddressCardProps {
    address: Address;
    onDelete: () => void;
}

export default function AddressCard({ address, onDelete }: AddressCardProps) {
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [idToDelete, setIdToDelete] = useState<string | null>(null);
    const [isDeleting, setIsDeleting] = useState(false);
    const router = useRouter();
    const [alert, setAlert] = useState({ show: false, message: '', type: 'success' as 'success' | 'error' });
    const { getToken } = useAuth();

    const handleEdit = () => {
        router.push("/address?id=" + address.id);
    };

    const handleDeleteClick = (id: string) => {
        setIdToDelete(id);
        setIsDeleteModalOpen(true);
    };

    const handleConfirmDelete = async () => {
        if (!idToDelete) return;

        try {
            const token = getToken();
            const response = await fetch(`${API_CONFIG.BASE_URL}/address/${idToDelete}`, {
                method: 'DELETE',
                headers: { Authorization: `Bearer ${token}` }
            });

            if (!response.ok) throw new Error("Erro ao excluir");

            setAlert({ show: true, message: "Endereço excluído com sucesso!", type: 'success' });
            onDelete();
            // Atualize sua lista local aqui ou redirecione
        } catch (error) {
            setAlert({ show: true, message: "Falha ao excluir endereço", type: 'error' });
        } finally {
            setIsDeleting(false);
            setIsDeleteModalOpen(false);
            setIdToDelete(null);
        }
    };


    return <div className="bg-white p-4 rounded-xl border border-gray-200 shadow-sm hover:border-blue-300 transition-colors group">
        <div className="flex justify-between items-start">
            {address.isDefault ?
                (
                    <span className="text-xs font-bold uppercase tracking-wider text-blue-500 bg-blue-50 px-2 py-0.5 rounded">
                        Principal
                    </span>
                )
                :
                (
                    <span className="text-xs font-bold uppercase tracking-wider text-gray-400 bg-gray-50 px-2 py-0.5 rounded">
                        Secundário
                    </span>
                )
            }


            <div className="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                <button onClick={() => handleDeleteClick(address.id)} className="cursor-pointer p-1 text-gray-400 hover:text-blue-600" title="Editar">
                    <Trash size={14} />
                </button>
                <button onClick={handleEdit} className="cursor-pointer p-1 text-gray-400 hover:text-blue-600" title="Editar">
                    <Edit2 size={14} />
                </button>
            </div>
        </div>
        <p className="mt-3 text-sm font-bold text-gray-800">{address.fullName}</p>
        <p className="text-sm font-medium text-gray-800">CPF: {address.cpf}</p>
        <p className="text-sm font-medium text-gray-800">{address.street}, {address.number}</p>
        <p className="text-sm text-gray-500">{address.neighborhood} — {address.city}, {address.state}</p>
        <p className="text-xs text-gray-400 mt-1">CEP: {address.zipCode}</p>

        <ConfirmDeleteModal
            isOpen={isDeleteModalOpen}
            isLoading={isDeleting}
            onClose={() => setIsDeleteModalOpen(false)}
            onConfirm={handleConfirmDelete}
        />
        <SimpleAlert
            isOpen={alert.show}
            message={alert.message}
            type={alert.type}
            onClose={() => setAlert({ ...alert, show: false })}
        />

    </div>;
}