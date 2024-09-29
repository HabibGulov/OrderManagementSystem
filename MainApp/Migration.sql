create table customers (
    id uuid primary key,
    full_name varchar(255) unique not null,
    email varchar(255) unique not null,
    phone varchar(50),
    created_at date not null
);
create table products (
    id uuid primary key,
    name varchar(255) unique not null,
    price numeric not null,
    stock_quantity int not null,
    created_at date not null
);
create table orders (
    id uuid primary key,
    customer_id uuid not null,
    total_amount numeric not null,
    order_date date not null,
    status varchar(50) not null,
    created_at date not null,
    foreign key (customer_id) references customers(id)
);
create table order_items (
    id uuid primary key,
    order_id uuid not null,
    product_id uuid not null,
    quantity int not null,
    price numeric not null,
    created_at date not null,
    foreign key (order_id) references orders(id),
    foreign key (product_id) references products(id)
);