echo 'Stopping old stack...'
echo 'Stopping db...'
docker stop scorp_stack_db

echo 'Stopping backend...'
docker stop scorp_stack_backend

echo 'Stopping admin dashboard...'
docker scorp_stack_admin_dashboard

echo 'Removing old stack...'
docker rm scorp_stack_admin_dashboard
docker rm scorp_stack_backend
docker rm scorp_stack_db

echo 'Removing old containers...'
docker rmi scorp_backend

echo 'Building backend docker image...'
docker build -t scorp_backend -f ./BackendDockerfile .

echo 'Building dashoard docker image...'
docker build -t scorp_admin_dashboard -f ./AdminDashboardDockerfile .

echo 'Building scorp stack...'
docker-compose up -d

echo 'Initializating DB...'
sleep 8
psql.exe -h 127.0.0.1 -d scorp -U postgres -W -p 2345 -a -q -f generate_database.sql
psql.exe -h 127.0.0.1 -d scorp -U postgres -W -p 2345 -a -q -f fill_database.sql