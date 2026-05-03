


export const CustomerCreatedAtHelper = {
    formatDate(date: string): string {
        return new Date(date).toLocaleDateString("en-US", {
            year: "numeric",
            month: "short",
            day: "2-digit",
        })
    },
}