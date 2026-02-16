import { useCart } from '@/context/CartContext';
import { Menu, ShoppingCart } from 'lucide-react';
import Link from 'next/link';
import Button from './Button';
import LoginStatus from './LoginStatus';
import MobileSidebar from './MobileSidebar';

export default function Header() {

    const { totalItens } = useCart();

    return (
        <header className="fixed top-0 left-0 w-full z-50">
            <nav className='flex justify-between items-center h-12 px-2 shadow-md bg-gray-800 text-white'>
                <div className="z-10">
                    <MobileSidebar></MobileSidebar>
                </div>
                <div className="absolute left-1/2 -translate-x-1/2 md:static md:translate-x-0 md:mr-auto">
                    <Link href="/" className="text-lg font-bold">
                        <Button>Lojinha.com</Button>
                    </Link>
                </div>
                <div>
                    <div className='flex'>
                        <div className='hidden md:block'>
                            <LoginStatus></LoginStatus>
                        </div>

                        <Link href="/cart">
                            <button
                                className='cursor-pointer flex items-center gap-2 bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg transition-colors'>
                                <ShoppingCart></ShoppingCart>
                                <span className='hidden md:block'>Carrinho</span>
                                <span className=''>{totalItens}</span>
                            </button>
                        </Link>

                    </div>
                </div>

            </nav>
        </header >
    )
}