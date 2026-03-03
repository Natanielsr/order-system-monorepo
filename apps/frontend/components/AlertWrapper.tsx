// app/AlertWrapper.tsx
"use client";

import { useState, useEffect } from "react";
import SimpleAlert from "./SimpleAlert";

interface Props {
    show: boolean;
    message: string;
}

export default function AlertWrapper({ show, message }: Props) {
    const [isOpen, setIsOpen] = useState(false);

    useEffect(() => {
        if (show) {
            setIsOpen(true);
        }
    }, [show]);

    return (
        <SimpleAlert
            message={message}
            isOpen={isOpen}
            onClose={() => setIsOpen(false)}
            type="error"
        />
    );
}
