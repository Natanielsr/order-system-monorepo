import { Product } from "@/types/product";

interface ProductImageProps {
    product: Product; // Recebe os dados vindos do servidor
    height: number;
    width: number;
}

export default function ProductImage({ product, height, width }: ProductImageProps) {
    // Usamos uma variável para o domínio (facilita mudar depois)
    const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5012/";

    const imageSrc = product.imagePath
        ? `${baseUrl}${product.imagePath}`
        : "/no-image.jpg";

    return (
        <img
            src={imageSrc}
            alt={product.name}
            // Usamos style para garantir que a largura/altura personalizada funcione
            style={{ height: `${height}px`, width: `${width}px` }}
            className="border border-gray-200 object-contain rounded-md shadow-md"
        />
    );
}