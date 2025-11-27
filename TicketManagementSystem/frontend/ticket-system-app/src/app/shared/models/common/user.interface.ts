 // Interface TypeScript para el modelo User del sistema de tickets
//
// Propiedades que coinciden con el backend C#:
// - id: number (corresponde a int Id en C#)
// - email: string (validar formato email en frontend)
// - fullName: string (nombre completo del usuario)
// - role: 'Admin' | 'Agent' | 'User' (union type para type safety)
// - isActive: boolean (estado del usuario)
// - createdAt: Date (fecha de creación, convertir desde ISO string)
// - updatedAt: Date (última actualización)
//
// Usar export para que sea reutilizable en toda la app
// Seguir convenciones TypeScript: camelCase para propiedades
export interface User {
    id: number;
    email: string;
    fullName: string;
    role: 'Admin' | 'Agent' | 'User';
    isActive: boolean;
    createdAt: Date;
    updatedAt: Date;
}
