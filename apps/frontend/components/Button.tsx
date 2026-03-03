"use client";

import { ReactNode, ButtonHTMLAttributes } from "react";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
    children: ReactNode;
}

export default function Button({
    children,
    className = "",
    ...props
}: ButtonProps) {
    return (
        <button
            {...props}
            className={`
        bg-white text-black
        px-2 py-2
        rounded-4xl
        border border-black
        shadow-sm
        transition-all duration-200
        hover:shadow-md
        active:scale-95
        focus:outline-none
        focus:ring-2 focus:ring-black/30
        ${className}
      `}
        >
            {children}
        </button>
    );
}
