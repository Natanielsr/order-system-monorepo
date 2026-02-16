import { OrderItem } from "./orderItem";

export type Order = {
    id: string;
    creationDate: string;
    total: number;
    status: number;
    orderProducts: OrderItem[]
};