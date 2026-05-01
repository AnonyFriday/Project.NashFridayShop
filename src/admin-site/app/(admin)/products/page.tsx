"use client";

import { useGetProductsQuery } from "@/features/products/product.api";

export default function ProductsPage() {
  const { data, isLoading, error } = useGetProductsQuery({
    pageIndex: 0,
    pageSize: 10,
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error...</div>;
  }

  return (
    <div>
      Products
      <p>{JSON.stringify(data)}</p>
    </div>
  );
}
