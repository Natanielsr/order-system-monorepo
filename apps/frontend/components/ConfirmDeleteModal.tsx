'use client'

interface ConfirmDeleteModalProps {
    isOpen: boolean;
    onClose: () => void;
    onConfirm: () => void;
    title?: string;
    message?: string;
    isLoading?: boolean;
}

export default function ConfirmDeleteModal({
    isOpen,
    onClose,
    onConfirm,
    title = "Confirmar Exclusão",
    message = "Tem certeza que deseja excluir este item? Esta ação não pode ser desfeita.",
    isLoading = false
}: ConfirmDeleteModalProps) {

    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm">
            <div className="bg-white rounded-lg shadow-xl max-w-md w-full p-6 animate-in fade-in zoom-in duration-200">
                <h2 className="text-xl font-bold text-gray-800">{title}</h2>
                <p className="mt-3 text-gray-600">
                    {message}
                </p>

                <div className="mt-6 flex justify-end gap-3">
                    <button
                        onClick={onClose}
                        disabled={isLoading}
                        className="px-4 py-2 text-sm font-medium text-gray-700 bg-gray-100 rounded-md hover:bg-gray-200 disabled:opacity-50 transition-colors"
                    >
                        Cancelar
                    </button>
                    <button
                        onClick={onConfirm}
                        disabled={isLoading}
                        className="px-4 py-2 text-sm font-medium text-white bg-red-600 rounded-md hover:bg-red-700 disabled:opacity-50 flex items-center gap-2 transition-colors"
                    >
                        {isLoading ? (
                            <>
                                <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                                Excluindo...
                            </>
                        ) : (
                            "Excluir"
                        )}
                    </button>
                </div>
            </div>
        </div>
    );
}