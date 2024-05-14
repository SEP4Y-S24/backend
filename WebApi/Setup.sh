set -e

until /root/.dotnet/tools/dotnet-ef migrations add InitialMigrations --no-build; do
>&2 echo "Postgres SQL is starting up"
sleep 1
done

until /root/.dotnet/tools/dotnet-ef database update --no-build; do
>&2 echo "Postgres SQL is starting up"
sleep 1
done

>&2 echo "Postgres SQL  is up - executing command"

/root/.dotnet/tools/dotnet-ef database update
