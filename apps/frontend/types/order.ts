import { OrderItem } from "./orderItem";
import { PaymentInfo } from "./paymentInfo";

export type Order = {
    id: string;
    creationDate: string;
    total: number;
    status: number;
    orderProducts: OrderItem[];
    paymentInfo: PaymentInfo[];
    code: string;
};