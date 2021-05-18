<?php
$servername = "127.0.0.1";
$database = "plotterdata"; 
$username = "AddPlotters";
$password = "tpaZHvd9s8iyyO24";
$sql = "mysql:host=$servername;dbname=$database;";
$dsn_Options = [PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION];
// Create a new connection to the MySQL database using PDO, $my_Db_Connection is an object
try { 
  $my_Db_Connection = new PDO($sql, $username, $password);
  echo "Connected successfully";
} catch (PDOException $error) {
  echo 'Connection error: ' . $error->getMessage();
}

if($_POST['postType'] == "Plotter")
{
  // Set the variables for the Plotter we want to add to the database
  $serial_number = $_POST['serial_number'];
  $model_id = $_POST['model_id'];
  $meters_printed = $_POST['meters_printed'];
  $datetime = date("Y-m-d H:i:s");
  $naam = $_POST['naam'];
  $IP = $_POST['IP'];
  // Here we create a variable that calls the prepare() method of the database object
  // The SQL query you want to run is entered as the parameter, and placeholders are written like this :placeholder_name
  $my_Insert_Statement = $my_Db_Connection->prepare("INSERT INTO `printer_data` (serial_number, model_id, meters_printed, datetime, naam, IP) VALUES (:serial_number, :model_id, :meters_printed, :datetime, :naam, :IP)");
  // Now we tell the script which variable each placeholder actually refers to using the bindParam() method
  // First parameter is the placeholder in the statement above - the second parameter is a variable that it should refer to
  $my_Insert_Statement->bindParam(":serial_number", $serial_number);
  $my_Insert_Statement->bindParam(":model_id", $model_id);
  $my_Insert_Statement->bindParam(":meters_printed", $meters_printed);
  $my_Insert_Statement->bindParam(":datetime", $datetime);
  $my_Insert_Statement->bindParam(":naam", $naam);
  $my_Insert_Statement->bindParam(":IP", $IP);
  // Execute the query using the data we just defined
  if ($my_Insert_Statement->execute()) {
    echo "New record created successfully";
  } else {
    echo "Unable to create record";
  }
}
elseif($_POST['postType'] == "Cartridge")
{
  // Set the variables for the Cartridge we want to add to the database
  $parent_id = $_POST['parent_id'];
  $cartridge_model = $_POST['cartridge_model'];
  $volume = $_POST['volume'];
  $max_volume = $_POST['max_volume'];
  $datetime = date("Y-m-d H:i:s");
  // Here we create a variable that calls the prepare() method of the database object
  // The SQL query you want to run is entered as the parameter, and placeholders are written like this :placeholder_name
  $my_Insert_Statement = $my_Db_Connection->prepare("INSERT INTO `cartridge_reading` (parent_id, cartridge_model, volume, max_volume, datetime) VALUES (:parent_id, :cartridge_model, :volume, :max_volume, :datetime)");
  // Now we tell the script which variable each placeholder actually refers to using the bindParam() method
  $my_Insert_Statement->bindParam(":parent_id", $parent_id);
  $my_Insert_Statement->bindParam(":cartridge_model", $cartridge_model);
  $my_Insert_Statement->bindParam(":volume", $volume);
  $my_Insert_Statement->bindParam(":datetime", $datetime);
  $my_Insert_Statement->bindParam(":max_volume", $max_volume);
  // Execute the query using the data we just defined
  // The execute() method returns TRUE if it is successful and FALSE if it is not, allowing you to write your own messages here
  if ($my_Insert_Statement->execute()) {
    echo "New record created successfully";
  } else {
    echo "Unable to create record";
  }
}
elseif ($_POST['postType'] == "Users") {
  // Set the variables for the person we want to add to the database
  $bedrijfs_Naam = $_POST['bedrijfs_Naam'];
  $contactpersoon = $_POST['contactpersoon'];
  $email = $_POST['email'];
  $telefoonnummer = $_POST['telefoonnummer'];
  // Here we create a variable that calls the prepare() method of the database object
  // The SQL query you want to run is entered as the parameter, and placeholders are written like this :placeholder_name
  $my_Insert_Statement = $my_Db_Connection->prepare("INSERT INTO `users` (bedrijfs_Naam, contactpersoon, email, telefoonnummer) VALUES (:bedrijfs_Naam, :contactpersoon, :email, :telefoonnummer)");
  // Now we tell the script which variable each placeholder actually refers to using the bindParam() method
  $my_Insert_Statement->bindParam(":bedrijfs_Naam", $bedrijfs_Naam);
  $my_Insert_Statement->bindParam(":contactpersoon", $contactpersoon);
  $my_Insert_Statement->bindParam(":email", $email);
  $my_Insert_Statement->bindParam(":telefoonnummer", $telefoonnummer);
  // Execute the query using the data we just defined
  // The execute() method returns TRUE if it is successful and FALSE if it is not, allowing you to write your own messages here
  if ($my_Insert_Statement->execute()) {
    echo "New record created successfully";
  } else {
    echo "Unable to create record";
  }
}
