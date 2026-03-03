/**
 * Valida se um CPF é autêntico seguindo o algoritmo da Receita Federal.
 * @param cpf String contendo o CPF (com ou sem pontuação)
 * @returns boolean
 */
export const validateCpf = (cpf: string): boolean => {
    // 1. Remove caracteres não numéricos
    const cleanCpf = cpf.replace(/\D/g, '');

    // 2. Verifica se tem 11 dígitos ou se todos os dígitos são iguais (ex: 111.111.111-11)
    // CPFs de números repetidos passam no cálculo matemático, mas são inválidos.
    if (cleanCpf.length !== 11 || /^(\d)\1+$/.test(cleanCpf)) {
        return false;
    }

    const digits = cleanCpf.split('').map(Number);

    // 3. Cálculo do primeiro dígito verificador
    const calculateDigit = (slice: number[], factor: number): number => {
        const sum = slice.reduce((acc, num, idx) => acc + num * (factor - idx), 0);
        const result = (sum * 10) % 11;
        return result === 10 || result === 11 ? 0 : result;
    };

    const firstDigit = calculateDigit(digits.slice(0, 9), 10);
    if (firstDigit !== digits[9]) return false;

    // 4. Cálculo do segundo dígito verificador
    const secondDigit = calculateDigit(digits.slice(0, 10), 11);
    if (secondDigit !== digits[10]) return false;

    return true;
};

/**
 * Valida se a senha atende aos requisitos:
 * - Pelo menos 8 caracteres
 * - Pelo menos uma letra maiúscula
 * - Pelo menos um caractere especial (!?*.@#$%^-)
 */
export function validatePassword(password: string, confirmPassword: string): { isValid: boolean; errors: string[] } {
    const errors: string[] = [];

    if (password.length < 8) {
        errors.push("A senha deve ter no mínimo 8 caracteres.");
    }

    if (!/[A-Z]/.test(password)) {
        errors.push("A senha deve conter pelo menos uma letra maiúscula.");
    }

    // Regex para os caracteres especiais específicos da sua Exception
    if (!/[!?*.@#$%^-]/.test(password)) {
        errors.push("A senha deve conter pelo menos um caractere especial (!?*.@#$%^-).");
    }

    if (password != confirmPassword) {
        errors.push("A senha e a confirmação de senha devem ser iguais.");
    }

    return {
        isValid: errors.length === 0,
        errors,
    };
}