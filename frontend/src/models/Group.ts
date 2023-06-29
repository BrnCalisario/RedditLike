export interface Group {
    name: string;
    description: string;
    ownerId : number;
    userParticipates: boolean;
    imageId: number | null;
    userQuantity: number;
}
