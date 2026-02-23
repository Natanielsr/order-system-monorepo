import { Address } from "@/types/address";
import { Edit2 } from "lucide-react";

interface AddressCardProps {
    address: Address
}

export default function AddressCard({ address }: AddressCardProps) {
    return <div className="bg-white p-4 rounded-xl border border-gray-200 shadow-sm hover:border-blue-300 transition-colors group">
        <div className="flex justify-between items-start">
            {address.default ?
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
                <button className="cursor-pointer p-1 text-gray-400 hover:text-blue-600" title="Editar">
                    <Edit2 size={14} />
                </button>
            </div>
        </div>
        <p className="mt-3 text-sm font-bold text-gray-800">{address.fullName}</p>
        <p className="text-sm font-medium text-gray-800">CPF: {address.cpf}</p>
        <p className="text-sm font-medium text-gray-800">{address.street}, {address.number}</p>
        <p className="text-sm text-gray-500">{address.neighborhood} — {address.city}, {address.state}</p>
        <p className="text-xs text-gray-400 mt-1">CEP: {address.zipCode}</p>
    </div>;
}