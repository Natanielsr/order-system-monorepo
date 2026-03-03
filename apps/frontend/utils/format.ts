// utils/format.ts
export const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL',
    }).format(value);
};

export const formatDateBR = (dateString: string) => {
    const date = new Date(dateString);

    return date.toLocaleString("pt-BR", {
        timeZone: "America/Sao_Paulo",
    });
};
