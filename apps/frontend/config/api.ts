// config/api.ts

// Define os endereços para cada ambiente
const ENV_URLS = {
    development: 'http://localhost:5012/api', // Sua API local
    production: 'https://api.seuservico.com/v1', // Sua API de produção
};

// Verifica em qual ambiente o código está rodando
const currentEnv = process.env.NODE_ENV === 'production' ? 'production' : 'development';

export const API_CONFIG = {
    BASE_URL: ENV_URLS[currentEnv],
};