select * from UserComments;

select * from AspNetUsers;

select * from AspNetUserLogins;

select * from AspNetUserRoles;

select * from AspNetRoles;

insert into AspNetRoles (Id, Name, NormalizedName) VALUES(1,'user', 'USER');
insert into AspNetRoles (Id, Name, NormalizedName) VALUES(2,'admin', 'ADMIN');


insert into AspNetUserRoles (UserId, RoleId) VALUES ('','1');


insert into AspNetUserRoles (UserId, RoleId) VALUES('ea5bc2ef-e443-46e3-aa0e-96aeef0cbcea', '1');

insert into AspNetUserRoles (UserId, RoleId) VALUES('4a14786c-642d-4a92-ae07-60aaa64ca394', '1');
insert into AspNetUserRoles (UserId, RoleId) VALUES('4a14786c-642d-4a92-ae07-60aaa64ca394', '2');

insert into AspNetUserRoles (UserId, RoleId) VALUES('fcb5f9a5-feb0-454b-9acf-9bac28cde9ec', '1');
insert into AspNetUserRoles (UserId, RoleId) VALUES('fcb5f9a5-feb0-454b-9acf-9bac28cde9ec', '2');


delete from AspNetRoles;
delete from AspNetUserRoles
