<?php

if ($_POST['check_data']) {
	
/* проверка данных */

$cur_login = $_POST['login'] . "---";
$cur_password = "---" . $_POST['password'];

$fd = fopen("passwords.txt", 'r');
	
while (!feof($fd)) {
		    
$str = htmlentities(fgets($fd));

if ( strpos($str, "test") !== False ) {
	
if ( strpos($str, "pass") !== False ) {
	
/* пользователь найден */

echo "user_exist";	
exit();
}	
	
}	

}

echo "user_not_exist";
exit();

}

if ($_POST['check_data_auth']) {
	
/* проверка данных */

$cur_login = $_POST['login'];
$cur_password = $_POST['password'];

if ( ($cur_login == "admin") && ($cur_password == "admin_password") ) {
	
echo "user_exist";	
exit(); 	

} else {
	
echo "user_not_exist";
exit();	

}

}

if ($_POST['add_data']) {
	
$cur_login = $_POST['login'] . "---";
$cur_password = $_POST['password'];


/* проверка существует ли такой логин */

$fd = fopen("passwords.txt", 'r');
	
while (!feof($fd)) {
		    
$str = htmlentities(fgets($fd));

if ( strpos($str, $cur_login) !== False ) {
	
echo "data_exist";	
exit();
}	

}

$str = $cur_login . $cur_password . "\n";

file_put_contents("passwords.txt", $str, FILE_APPEND);

echo "data_added";
exit();
	
}

if ($_POST['delete_data']) {
	
$cur_login = $_POST['login'] . "---";

$fd = fopen("passwords.txt", 'r');

$itog_str = "";
	
while (!feof($fd)) {
		    
$str = htmlentities(fgets($fd));

if ( strpos($str, $cur_login) !== False ) {
	
/* эту строку нужно удалить */	
	
} else {
	
$itog_str .= $str . "\n";	
}	

}
	
$fd = fopen("passwords.txt", 'w');
fwrite($fd, $itog_str);
fclose($fd);	
	
echo "data_deleted";	
exit();
	
}

?>