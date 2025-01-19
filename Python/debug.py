from datetime import datetime as dt
from faker import Faker

fake = Faker('id_ID')

# print(fake.date_time_between(start_date='-3 days', end_date='now').date())
# print(fake.date_between(start_date='-3d', end_date='now'))

# now = dt.now()
# print(now.year)

data = (
        f"{dt.now().year}-{str(fake.random_int(12, 20)).zfill(4)}",
        f"{fake.date_between(start_date='-6d', end_date='now')}",
        f"{fake.random_int(1, 10)}",
        f"{0}"
    )

print(data)