import { Order } from "@/types/order";

export const getStatusColor = (status: Order["status"]) => {
    switch (status) {
        case 0: //Pendente
            return "bg-yellow-100 text-yellow-700";
        case 1: //Pago
            return "bg-green-100 text-green-700";
        case 2: //Enviado
            return "bg-blue-100 text-blue-700";
        case 3: //Cancelado
            return "bg-red-100 text-red-700";
    }
}
export const getStatusName = (status: Order["status"]) => {
    switch (status) {
        case 0: //Pendente
            return "Pendente";
        case 1: //Pago
            return "Pago";
        case 2: //Enviado
            return "Enviado";
        case 3: //Cancelado
            return "Cancelado";
    }
}