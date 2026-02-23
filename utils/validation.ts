// Validação básica de CPF
export const validateCPF = (cpf: string) => {
    const cleanCPF = cpf.replace(/\D/g, "");
    if (cleanCPF.length !== 11) return false;
    // Aqui você pode adicionar a lógica de dígitos verificadores se desejar
    return !/^(\d)\1+$/.test(cleanCPF);
};

