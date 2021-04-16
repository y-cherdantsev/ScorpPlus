echo 'Stopping db...'
docker stop scorp_stack_db

echo 'Stopping backend...'
docker stop scorp_stack_backend

echo 'Stopping admin dashboard...'
docker stop scorp_stack_admin_dashboard

echo 'Starting db...'
docker start scorp_stack_db

echo 'Starting backend...'
docker start scorp_stack_backend

echo 'Starting admin dashboard...'
docker stop scorp_stack_admin_dashboard