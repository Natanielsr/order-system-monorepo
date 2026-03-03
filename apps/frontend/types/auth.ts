// types/auth.ts
export interface UserClaims {
    nameid: string;          // Geralmente o ID do usuário
    unique_name: string;     // O campo que você definiu
    email: string;
    role?: string;        // Opcional: admin, user, etc.
    nbf: number;
    exp: number;          // Timestamp de expiração
    iat: number;
    iss?: string;         // Emissor (Issuer)
    aud: string;
    [key: string]: any;   // Para outras claims customizadas que possam vir
}