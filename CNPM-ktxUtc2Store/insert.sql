use ktxUtc2Store;
insert into category(categoryName)values(N'Giay')

insert into category(categoryName)values(N'Thời trang nam')

insert into category(categoryName)values(N'Thời trang nữ')

insert into product(productName,description,discount,price,imageUrl,categoryId,qty_inStock)
values(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,65000,N'giay1.jpg',1,55),
(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,41900,N'giay2.jpg',1,45),
(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,89000,N'giay3.jpg',1,35),
(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,109000,N'giay4.jpg',1,25)
insert into product(productName,description,discount,price,imageUrl,categoryId,qty_inStock)
values(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,65000,N'donam1.jpg',2,50),
(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,41900,N'donam5.jpg',2,40),
(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,89000,N'donam3.jpg',2,30),
(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,109000,N'donam4.jpg',2,20)

insert into product(productName,description,discount,price,imageUrl,categoryId,qty_inStock)
values(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,65000,N'donu4.jpg',3,10),
(N'Giày Asics Court MZ',N'Giày Asics Court MZ Cream Black Gum Nam Nữ 36-43, giày Asics Court bản S.Cấp đế Gum full phụ kiện',0,41900,N'donu5.jpg',3,10)

insert into variation(name,value,categoryId)values
(N'size',N'36',1),
(N'size',N'37',1),
(N'size',N'38',1),
(N'size',N'39',1),
(N'size',N'40',1),
(N'size',N'41',1),
(N'color',N'đen',1),
(N'color',N'trắng',1),
(N'color',N'nâu',1)

insert into variation(name,value,categoryId)values
(N'size',N'S',2),
(N'size',N'M',2),
(N'size',N'L',2),
(N'size',N'XL',2),
(N'size',N'XXL',2),
(N'color',N'đen',2),
(N'color',N'trắng',2),
(N'color',N'hồng',2)

insert into variation(name,value,categoryId)values
(N'size',N'S',3),
(N'size',N'M',3),
(N'size',N'L',3),
(N'size',N'XL',3),
(N'size',N'XXL',3),
(N'color',N'đen',3),
(N'color',N'trắng',3),
(N'color',N'hồng',3)

insert into orderStatus(statusName)values(N'Đã đặt hàng')
insert into orderStatus(statusName)values(N'Đặt hàng thành công')
insert into orderStatus(statusName)values(N'Đang vận chuyển')
insert into orderStatus(statusName)values(N'Đã nhận hàng')
insert into orderStatus(statusName)values(N'Đã hủy đơn')

insert into productVariations(productId,variationId)values(1,1),(1,2),(1,3),(1,4),(1,5),(1,6),(1,7),(1,8),(1,9)
insert into productVariations(productId,variationId)values(2,1),(2,2),(2,3),(2,4),(2,5),(2,6),(2,7),(2,8)
insert into productVariations(productId,variationId)values(3,1),(3,2),(3,3),(3,4),(3,5),(3,6),(3,7),(3,8)

