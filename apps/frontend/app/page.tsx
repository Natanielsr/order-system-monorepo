// app/page.tsx
import AlertWrapper from '@/components/AlertWrapper';
import ProductList from '@/components/ProductList';
import { Product } from '@/types/product';


var isError = false;

async function getProducts(): Promise<Product[]> {
  try {
    const response = await fetch('http://localhost:5012/api/product', {
      // next: { revalidate: 3600 } // Opcional: atualiza o cache a cada 1 hora
      cache: 'no-store' // Use isso se os preços/estoque mudarem constantemente
    });

    if (!response.ok) {
      isError = true;
      console.error(response);
      throw new Error('Falha ao buscar produtos');
    }

    isError = false;
    return response.json();

  } catch (error) {
    isError = true;
    console.log(error);
    return [];
  }

}

export default async function Home() {

  const products = await getProducts();
  console.log(products);
  // const { cart, addToCart, removeFromCart, total } = useCart();

  return (
    <main className="pt-10">
      <ProductList initialProducts={products}></ProductList>
      <AlertWrapper
        show={isError}
        message="Erro Ao Buscar Produtos!"
      />
    </main>
  );
}